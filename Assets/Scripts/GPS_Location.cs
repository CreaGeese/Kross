using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class GPS_Location : MonoBehaviour
{
    [SerializeField] Text GPSstatus;
    [SerializeField] Text GPSLatitudeValue;
    [SerializeField] Text GPSLongitudeValue;
    [SerializeField] Text GPSAlttitudeValue;
    [SerializeField] Text HorizontalAccuracyValue;
    [SerializeField] Text TimeStampValue;
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(GPS()); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GPS()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            GPSstatus.text = "Time Out";
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSstatus.text = "Unable to determine device location";
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            GPSstatus.text = "Running";
            InvokeRepeating("UpdateGPS",0.5f,1f);
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            //Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

        }

        // Stops the location service if there is no need to query location updates continuously.
        //Input.location.Stop();
    }

    private void UpdateGPS()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            GPSstatus.text = "Running";
            GPSLatitudeValue.text = Input.location.lastData.latitude.ToString();
            GPSLongitudeValue.text = Input.location.lastData.longitude.ToString();
            GPSAlttitudeValue.text = Input.location.lastData.altitude.ToString();
            HorizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            TimeStampValue.text = Input.location.lastData.timestamp.ToString();
        }
        else
        {
            GPSstatus.text = "Stop";
        }
    }
}
