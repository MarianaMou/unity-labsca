using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class bitalino : MonoBehaviour
{
    // Class Variables
    private PluxDeviceManager pluxDevManager;

    // GUI Objects.
    
    public Text OutputMsgText;

    // Class constants (CAN BE EDITED BY IN ACCORDANCE TO THE DESIRED DEVICE CONFIGURATIONS)
    [System.NonSerialized]
    public List<string> domains = new List<string>() { "BTH" };

    //changé
    public int samplingRate = 1000;
    private int BitalinoPID = 1538;
 

    // Start is called before the first frame update
    private void Start()
    {
        // Initialise object
        pluxDevManager = new PluxDeviceManager(ScanResults, ConnectionDone, AcquisitionStarted, OnDataReceived, OnEventDetected, OnExceptionRaised);

        // Important call for debug purposes by creating a log file in the root directory of the project.
       // pluxDevManager.WelcomeFunctionUnity();

        // ScanButtonFunction();
        // ConnectButtonFunction();
        pluxDevManager.GetDetectableDevicesUnity(domains);

        pluxDevManager.PluxDev("98:D3:B1:FD:3C:D0");
        Debug.Log("connect2");
        StartButtonFunction();
    }

    // Update function, being constantly invoked by Unity.
    private void Update()
    { }

    // Method invoked when the application was closed.
    private void OnApplicationQuit()
    {
        try
        {
            // Disconnect from device.
            if (pluxDevManager != null)
            {
                pluxDevManager.DisconnectPluxDev();
                Console.WriteLine("Application ending after " + Time.time + " seconds");
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine("Device already disconnected when the Application Quit.");
        }
    }

    /**
     * =================================================================================
     * ============================= GUI Events ========================================
     * =================================================================================
     */

    // Method called when the "Scan for Devices" button is pressed.
    public void ScanButtonFunction()
    {
        // Search for PLUX devices
        pluxDevManager.GetDetectableDevicesUnity(domains);
        Debug.Log("scan");

        // Disable the "Scan for Devices" button.
        //ScanButton.interactable = false;
    }

    // Method called when the "Connect to Device" button is pressed.
    public void ConnectButtonFunction()
    {
        // Disable Connect button.
        //ConnectButton.interactable = false;
        Debug.Log("connect");

        // Connect to the device selected in the Dropdown list.
        //pluxDevManager.PluxDev(DeviceDropdown.options[DeviceDropdown.value].text);
        pluxDevManager.PluxDev("98:D3:B1:FD:3C:D0");
    }

    // Method called when the "Disconnect Device" button is pressed.
    public void DisconnectButtonFunction()
    {
        // Disconnect from the device.
        pluxDevManager.DisconnectPluxDev();


    }

    // Method called when the "Start Acquisition" button is pressed.
    public void StartButtonFunction()
    {
        Debug.Log("ca start?");
        // Get the Sampling Rate and Resolution values.
        //samplingRate = int.Parse(SamplingRateDropdown.options[SamplingRateDropdown.value].text);
        samplingRate = 1000;
        //int resolution = 16;

        // Initializing the sources array.
        List<PluxDeviceManager.PluxSource> pluxSources = new List<PluxDeviceManager.PluxSource>();
       
        // BITalino (2 Analog sensors)
        if (pluxDevManager.GetProductIdUnity() == BitalinoPID)
        {
            // Starting a real-time acquisition from:
            // >>> BITalino [Channels A2 and A5 active]
            pluxDevManager.StartAcquisitionUnity(samplingRate, new List<int> { 2 }, 10);
        }
        else
        {
            // Start a real-time acquisition with the created sources.
            pluxDevManager.StartAcquisitionBySourcesUnity(samplingRate, pluxSources.ToArray());
        }
    }

    // Method called when the "Stop Acquisition" button is pressed.
    public void StopButtonFunction()
    {
        // Stop the real-time acquisition.
        pluxDevManager.StopAcquisitionUnity();

      
    }

    /**
     * =================================================================================
     * ============================= Callbacks =========================================
     * =================================================================================
     */

    // Callback that receives the list of PLUX devices found during the Bluetooth scan.
    public void ScanResults(List<string> listDevices)
    {
        

        if (listDevices.Count > 0)
        {
           
            Debug.Log(listDevices);
         

            // Show an informative message about the number of detected devices.
            OutputMsgText.text = "Scan completed.\nNumber of devices found: " + listDevices.Count;
        }
        else
        {
            // Show an informative message stating the none devices were found.
            OutputMsgText.text = "Bluetooth device scan didn't found any valid devices.";
        }
    }

    // Callback invoked once the connection with a PLUX device was established.
    // connectionStatus -> A boolean flag stating if the connection was established with success (true) or not (false).
    public void ConnectionDone(bool connectionStatus)
    {
        if (connectionStatus)
        {

        }
        else
        {

            // Show an informative message stating the connection with the device was not established with success.
            OutputMsgText.text = "It was not possible to establish a connection with the device. Please, try to repeat the connection procedure.";
        }
    }

    // Callback invoked once the data streaming between the PLUX device and the computer is started.
    // acquisitionStatus -> A boolean flag stating if the acquisition was started with success (true) or not (false).
    // exceptionRaised -> A boolean flag that identifies if an exception was raised and should be presented in the GUI (true) or not (false).
    public void AcquisitionStarted(bool acquisitionStatus, bool exceptionRaised = false, string exceptionMessage = "")
    {
        if (acquisitionStatus)
        {

        }
        else
        {
            // Present an informative message about the error.
            OutputMsgText.text = !exceptionRaised ? "It was not possible to start a real-time data acquisition. Please, try to repeat the scan/connect/start workflow." : exceptionMessage;

        }
    }

    // Callback invoked every time an exception is raised in the PLUX API Plugin.
    // exceptionCode -> ID number of the exception to be raised.
    // exceptionDescription -> Descriptive message about the exception.
    public void OnExceptionRaised(int exceptionCode, string exceptionDescription)
    {
        if (pluxDevManager.IsAcquisitionInProgress())
        {
            // Present an informative message about the error.
            OutputMsgText.text = exceptionDescription;


        }
    }

    // Callback that receives the data acquired from the PLUX devices that are streaming real-time data.
    // nSeq -> Number of sequence identifying the number of the current package of data.
    // data -> Package of data containing the RAW data samples collected from each active channel ([sample_first_active_channel, sample_second_active_channel,...]).
    public void OnDataReceived(int nSeq, int[] data)
    {
        // Show samples with a 1s interval.
        if (nSeq % samplingRate == 0)
        {
            // Show the current package of data.
            string outputString = "Acquired Data:\n";
            for (int j = 0; j < data.Length; j++)
            {
                outputString += data[j] + "\t";
                Debug.Log(outputString + data[j]);
            }

            // Show the values in the GUI.
            OutputMsgText.text = outputString;
        }
    }

    // Callback that receives the events raised from the PLUX devices that are streaming real-time data.
    // pluxEvent -> Event object raised by the PLUX API.
    public void OnEventDetected(PluxDeviceManager.PluxEvent pluxEvent)
    {
        if (pluxEvent is PluxDeviceManager.PluxDisconnectEvent)
        {
            // Present an error message.
            OutputMsgText.text =
                "The connection between the computer and the PLUX device was interrupted due to the following event: " +
                (pluxEvent as PluxDeviceManager.PluxDisconnectEvent).reason;

            // Securely stop the real-time acquisition.
            pluxDevManager.StopAcquisitionUnity(-1);

 
        }
        else if (pluxEvent is PluxDeviceManager.PluxDigInUpdateEvent)
        {
            PluxDeviceManager.PluxDigInUpdateEvent digInEvent = (pluxEvent as PluxDeviceManager.PluxDigInUpdateEvent);
            Console.WriteLine("Digital Input Update Event Detected on channel " + digInEvent.channel + ". Current state: " + digInEvent.state);
        }
    }

    /**
     * =================================================================================
     * ========================== Auxiliary Methods ====================================
     * =================================================================================
     */

    // Auxiliary method used to reboot the GUI elements.

}