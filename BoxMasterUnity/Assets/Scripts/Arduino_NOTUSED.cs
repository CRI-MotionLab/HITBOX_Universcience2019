using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Linq;

public class Arduino_NOTUSED : MonoBehaviour
{
	private IEnumerator coroutine;
	public SerialPort serial = new SerialPort ("COM7", 38400);
	private string serialData = "";
	private int dataCounter = 0;
	// count number of incoming data

	public int ROWS = 8;
	public int COLS = 8;

	public GameObject datapointPrefab;
	private GameObject[,] pointGrid;

	Vector3 acceleration = new Vector3 (0.0f, 0.0f, 0.0f);

	void Start ()
	{
		pointGrid = new GameObject[ROWS, COLS];
		for (int i = 0; i < ROWS; i++) {
			for (int j = 0; j < COLS; j++) {
				float x_ = j * 10;
				float y_ = i * 10;
				pointGrid [i, j] = GameObject.Instantiate (datapointPrefab, new Vector3 (x_, y_, 0), Quaternion.identity);
			}
		}

		try {
			serial.Open ();
			serial.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
		} catch {
			Debug.Log ("Could not open serial port");

		}


//		coroutine = getSerialData (0.02f);
//		StartCoroutine (coroutine);
		StartCoroutine (printSerialDataRate (1f));
	}

	void Update ()
	{
		this.getSerialData ();

		// Remap and display data points
		for (int i = 0; i < ROWS; i++) {
			// Get row data range
			float minRow_ = 1000.0f;
			float maxRow_ = -1000.0f;
			float sumRow_ = 0.0f;
			for (int j = 0; j < COLS; j++) {
				sumRow_ += pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal;

				if (minRow_ > pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal) {
					minRow_ = pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal;
				}
				if (maxRow_ < pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal) {
					maxRow_ = pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal;
				}
			}

			// Get remap values for the current row and display data point
			for (int j = 0; j < COLS; j++) {
				if (maxRow_ - minRow_ != 0) {
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal = (pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal - minRow_) / (maxRow_ - minRow_);
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal *= sumRow_;
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal /= 1024.0f; // 1024 = max analog range
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal = Mathf.Clamp (pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal, 0.0f, 1.0f);
				}
			}
		}
	}

	private void getSerialData ()
	{
//		while (true) {
//			yield return new WaitForSeconds (waitTime);

		if (this.serialData != "") {
			char adr_ = this.serialData.ToCharArray () [0];
			this.serialData = this.serialData.Split (adr_) [1];

			switch (adr_) {
			case 'z':
					// GET COORDINATES
				try {
					string[] cooStr_ = this.serialData.Split ('x');  // format = ROWixCOL1yCOL2yCOL3...yCOLj
					if (cooStr_.Length == 2) {
						int colIndex_ = int.Parse (cooStr_ [0]);
						int[] rowValues_ = cooStr_ [1].Split ('y').Select (str => int.Parse (str)).ToArray ();

						if (rowValues_.Length == ROWS) {
							for (int i = 0; i < ROWS; i++) {
								pointGrid [i, colIndex_].GetComponent<DatapointControl> ().PushNewRawVal (rowValues_ [i]);  // 1024 = max analog range
							}
							this.dataCounter++;
						}
					}
				} catch {
					print ("bad string format");
				}
				break;

			case 'a':
					// GET ACCELERATION
				try {
					int[] acc_ = this.serialData.Split ('c').Select (str => int.Parse (str)).ToArray (); // format = ACCXcACCYcACCZ
					if (acc_.Length == 3) {
						this.acceleration = new Vector3 (acc_ [0], acc_ [1], acc_ [2]);
						this.dataCounter++;
					}
				} catch {
					print ("bad string format");
				}
				break;

			default:
				break;
			}

			this.serialData = "";
		}
	}

	private void DataReceivedHandler (object sender, SerialDataReceivedEventArgs e)
	{
		SerialPort sp = (SerialPort)sender;
		this.serialData = sp.ReadLine ();
	}

	private IEnumerator printSerialDataRate (float waitTime)
	{
		while (true) {
			yield return new WaitForSeconds (waitTime);
			print ("Serial data rate = " + this.dataCounter / waitTime + " data/s");
			this.dataCounter = 0;
		}
	}
}
