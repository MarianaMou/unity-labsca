using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Hybrid8Test : MonoBehaviour
{
    // Class Variables
    private PluxDeviceManager pluxDevManager;

    // GUI Objects.
    public Button ScanButton;

    public Button ConnectButton;
    public Button DisconnectButton;
    public Button StartAcqButton;
    public Button StopAcqButton;
    public Dropdown DeviceDropdown;
    public Dropdown SamplingRateDropdown;
    public Dropdown ResolutionDropdown;
    public Dropdown RedIntensityDropdown;
    public Dropdown InfraredIntensityDropdown;
    public Text OutputMsgText;

    // Class constants (CAN BE EDITED BY IN ACCORDANCE TO THE DESIRED DEVICE CONFIGURATIONS)
    [System.NonSerialized]
    public List<string> domains = new List<string>() { "BTH" };

    public int samplingRate = 100;

    private int Hybrid8PID = 517;
    private int BiosignalspluxPID = 513;
    private int BitalinoPID = 1538;
    private int MuscleBanPID = 1282;
    private int MuscleBanNewPID = 2049;
    private int CardioBanPID = 2050;
    private int BiosignalspluxSoloPID = 532;
    private int MaxLedIntensity = 255;


    public int distanceMin = 10;
    public int compteurDistance = 0;
    public int seuil = 600;
    public int compteurPic = 0;
    public List<int> lesPics;
    public List<int> lesTemps;
    public int freq;
    public bool ajoutPic = false;
    public int moyen10RR = 10;
    public bool stresser = false;

    // Start is called before the first frame update
    private void Start()
    {
        CreerText("");
        // Initialise object
        pluxDevManager = new PluxDeviceManager(ScanResults, ConnectionDone, AcquisitionStarted, OnDataReceived, OnEventDetected, OnExceptionRaised);

        // Important call for debug purposes by creating a log file in the root directory of the project.
        pluxDevManager.WelcomeFunctionUnity();
    }

    // Update function, being constantly invoked by Unity.
    private void Update()
    { }

    void CreerText(string contenu)
    {
        string chemin = Application.dataPath + "/Log.txt";

        //est ce que ce fichier existe deja? si il exite pas alors on le créer
        if (!File.Exists(chemin))
        {
            File.WriteAllText(chemin, "Bitalino \n\n");
        }

        //ajouter du contenu
        if (contenu != "")
        {
            File.AppendAllText(chemin, contenu);
        }

    }

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

        // Disable the "Scan for Devices" button.
        ScanButton.interactable = false;
    }

    // Method called when the "Connect to Device" button is pressed.
    public void ConnectButtonFunction()
    {
        // Disable Connect button.
        ConnectButton.interactable = false;

        // Connect to the device selected in the Dropdown list.
        pluxDevManager.PluxDev(DeviceDropdown.options[DeviceDropdown.value].text);
    }

    // Method called when the "Disconnect Device" button is pressed.
    public void DisconnectButtonFunction()
    {
        // Disconnect from the device.
        pluxDevManager.DisconnectPluxDev();

        // Reboot GUI elements state.
        RebootGUI();
    }

    // Method called when the "Start Acquisition" button is pressed.
    public void StartButtonFunction()
    {
        // Get the Sampling Rate and Resolution values.
        samplingRate = 100;
        int resolution = int.Parse(ResolutionDropdown.options[ResolutionDropdown.value].text);

        // Initializing the sources array.
        List<PluxDeviceManager.PluxSource> pluxSources = new List<PluxDeviceManager.PluxSource>();

        // biosignalsplux Hybrid-8 device (3 sensors >>> 1 Analog + 2 Digital SpO2/fNIRS)
        if (pluxDevManager.GetProductIdUnity() == Hybrid8PID)
        {
            // Add the sources of the digital channels (CH1 and CH2).
            pluxSources.Add(new PluxDeviceManager.PluxSource(1, 1, resolution, 0x03));
            pluxSources.Add(new PluxDeviceManager.PluxSource(2, 1, resolution, 0x03));

            // Define the LED Intensities of both sensors (CH1 and CH2) as: {RED, INFRARED}
            int redLedIntensity = (int)(int.Parse(RedIntensityDropdown.options[RedIntensityDropdown.value].text) * (MaxLedIntensity / 100f)); // A 8-bit value (0-255)
            int infraredLedIntensity = (int)(int.Parse(InfraredIntensityDropdown.options[InfraredIntensityDropdown.value].text) * (MaxLedIntensity / 100f)); // A 8-bit value (0-255)
            int[] ledIntensities = new int[2] { redLedIntensity, infraredLedIntensity };
            pluxDevManager.SetParameter(1, 0x03, ledIntensities);
            pluxDevManager.SetParameter(2, 0x03, ledIntensities);

            // Add the source of the analog channel (CH8).
            pluxSources.Add(new PluxDeviceManager.PluxSource(8, 1, resolution, 0x01));

            // Add the sources of the internal IMU channels (CH11 with 9 derivations [3xACC | 3xGYRO | 3xMAG] defined by the 0x01FF chMask).
            int imuPort = 11;
            pluxSources.Add(new PluxDeviceManager.PluxSource(imuPort, 1, resolution, 0x01FF));

            // Alternatively only some of the derivations can be activated.
            // >>> 3xACC (channel mask 0x0007)
            // pluxSources.Add(new PluxDeviceManager.PluxSource(imuPort, 1, resolution, 0x0007));
            // >>> 3xGYR (channel mask 0x0038)
            // pluxSources.Add(new PluxDeviceManager.PluxSource(imuPort, 1, resolution, 0x0038));
            // >>> 3xMAG (channel mask 0x01C0)
            // pluxSources.Add(new PluxDeviceManager.PluxSource(imuPort, 1, resolution, 0x01C0));
        }
        // biosignalsplux (2 Analog sensors)
        else if (pluxDevManager.GetProductIdUnity() == BiosignalspluxPID)
        {
            // Starting a real-time acquisition from:
            // >>> biosignalsplux [CH1 and CH2 active]
            pluxSources.Add(new PluxDeviceManager.PluxSource(1, 1, resolution, 0x01)); // CH1 | EDA
            pluxSources.Add(new PluxDeviceManager.PluxSource(2, 1, resolution, 0x01)); // CH2 | ECG

            // Add the sources of the digital channel (CH9 | fNIRS/SpO2).
            // pluxSources.Add(new PluxDeviceManager.PluxSource(9, 1, resolution, 0x03));

            // Define the LED Intensities of the CH9 sensor as: {RED, INFRARED}
            // int redLedIntensity = (int)(int.Parse(RedIntensityDropdown.options[RedIntensityDropdown.value].text) * (MaxLedIntensity / 100f)); // A 8-bit value (0-255)
            // int infraredLedIntensity = (int)(int.Parse(InfraredIntensityDropdown.options[InfraredIntensityDropdown.value].text) * (MaxLedIntensity / 100f)); // A 8-bit value (0-255)
            // int[] ledIntensities = new int[2] { redLedIntensity, infraredLedIntensity };
            // pluxDevManager.SetParameter(9, 0x03, ledIntensities);
        }
        // muscleBAN (7 Analog sensors)
        else if (pluxDevManager.GetProductIdUnity() == MuscleBanPID)
        {
            // Starting a real-time acquisition from:
            // >>> muscleBAN [CH1 > EMG]
            pluxSources.Add(new PluxDeviceManager.PluxSource(1, 1, resolution, 0x01));
            // >>> muscleBAN [CH2-CH4 > ACC | CH5-CH7 > MAG active]
            pluxSources.Add(new PluxDeviceManager.PluxSource(2, 1, resolution, 0x3F));
        }
        // muscleBAN v2 (7 Analog sensors)
        else if (pluxDevManager.GetProductIdUnity() == MuscleBanNewPID)
        {
            // Starting a real-time acquisition from:
            // >>> muscleBAN [CH1 > EMG]
            pluxSources.Add(new PluxDeviceManager.PluxSource(1, 1, resolution, 0x01));
            // >>> muscleBAN Virtual Port [CH2-CH4 > ACC | CH5-CH7 > MAG active]
            pluxSources.Add(new PluxDeviceManager.PluxSource(11, 1, resolution, 0x3F));
        }
        // cardioBAN (7 Analog sensors)
        else if (pluxDevManager.GetProductIdUnity() == CardioBanPID)
        {
            // Starting a real-time acquisition from:
            // >>> cardioBAN [CH1 > ECG]
            pluxSources.Add(new PluxDeviceManager.PluxSource(1, 1, resolution, 0x01));
            // >>> cardioBAN Virtual Port [CH2-CH4 > ACC | CH5-CH7 > MAG active]
            pluxSources.Add(new PluxDeviceManager.PluxSource(11, 1, resolution, 0x3F));
        }
        // biosignalsplux Solo (8 Analog sensors)
        else if (pluxDevManager.GetProductIdUnity() == BiosignalspluxSoloPID)
        {
            // Starting a real-time acquisition from:
            // >>> biosignalsplux Solo [CH1 > MICRO]
            pluxSources.Add(new PluxDeviceManager.PluxSource(1, 1, resolution, 0x01));
            // >>> biosignalsplux Solo [CH2 > CUSTOM]
            pluxSources.Add(new PluxDeviceManager.PluxSource(2, 1, resolution, 0x01));
            // >>> biosignalsplux Solo Virtual Port [CH3-CH5 > ACC | CH6-CH8 > MAG]
            pluxSources.Add(new PluxDeviceManager.PluxSource(11, 1, resolution, 0x3F));
        }

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


        // Enable the "Start Acquisition" button and disable the "Stop Acquisition" button.
        StartAcqButton.interactable = true;
        StopAcqButton.interactable = false;
    }

    /**
     * =================================================================================
     * ============================= Callbacks =========================================
     * =================================================================================
     */

    // Callback that receives the list of PLUX devices found during the Bluetooth scan.
    public void ScanResults(List<string> listDevices)
    {
        // Enable the "Scan for Devices" button.
        ScanButton.interactable = true;

        if (listDevices.Count > 0)
        {
            // Update list of devices.
            DeviceDropdown.ClearOptions();
            DeviceDropdown.AddOptions(listDevices);
            Debug.Log(listDevices);

            // Enable the Dropdown and the Connect button.
            DeviceDropdown.interactable = true;
            ConnectButton.interactable = true;

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
            // Disable some GUI elements.
            ScanButton.interactable = false;
            DeviceDropdown.interactable = false;
            ConnectButton.interactable = false;

            // Enable some generic GUI elements.
            if (pluxDevManager.GetProductIdUnity() != BitalinoPID)
            {
                ResolutionDropdown.interactable = true;
            }

            SamplingRateDropdown.interactable = true;
            StartAcqButton.interactable = true;
            DisconnectButton.interactable = true;

            // Enable some biosignalsplux Hybrid-8 specific GUI elements.
            if (pluxDevManager.GetProductIdUnity() == Hybrid8PID || pluxDevManager.GetProductIdUnity() == BiosignalspluxPID)
            {
                RedIntensityDropdown.interactable = true;
                InfraredIntensityDropdown.interactable = true;
            }
        }
        else
        {
            // Enable Connect button.
            ConnectButton.interactable = true;

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
            // Enable the "Stop Acquisition" button and disable the "Start Acquisition" button.
            StartAcqButton.interactable = false;
            StopAcqButton.interactable = true;
        }
        else
        {
            // Present an informative message about the error.
            OutputMsgText.text = !exceptionRaised ? "It was not possible to start a real-time data acquisition. Please, try to repeat the scan/connect/start workflow." : exceptionMessage;

            // Reboot GUI.
            RebootGUI();
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

            // Reboot GUI.
            RebootGUI();
        }
    }

    // Callback that receives the data acquired from the PLUX devices that are streaming real-time data.
    // nSeq -> Number of sequence identifying the number of the current package of data.
    // data -> Package of data containing the RAW data samples collected from each active channel ([sample_first_active_channel, sample_second_active_channel,...]).
    public void OnDataReceived(int nSeq, int[] data)
    {
        // Show samples with a 1s interval.
        //Debug.Log("lenseq: " + nSeq + "sampling: " + samplingRate+ "nSeq % samplingRate le modulo "+ nSeq % samplingRate);
        if (nSeq % samplingRate == 0)
        {
            // Show the current package of data.
            string outputString = "Acquired Data:\n";
            for (int j = 0; j < data.Length; j++)
            {
                outputString += data[j] + "\t";
                //    Debug.Log("le data j : "+data[j]);
            }


            // Show the values in the GUI.
            OutputMsgText.text = outputString;
        }




        // donnée enregistre + ajouter ds log avec detection de pic
        for (int j = 0; j < data.Length; j++)
        {
            // detecte les pics suppéreieur au seuil
            //Debug.Log("compteurDsit (>10): " + compteurDistance+"cont de tem : "+ lesTemps.Count);
            if (data[j] > seuil && compteurDistance > distanceMin)

            {
                CreerText(data[j] + " pic \n");
                //ajouter les pics les 6 dernières sercondes + les temps ou les pic on été trouvée
                // si un pic apparait au dela de 6 sec d'intervalle on enlève ce pic dans lesPics et le temps corespondand ds lesTemps .
                if (lesTemps.Count > 0 && lesPics.Count > 0)
                {
                    // temps pour calculer la fréquence
                    while (nSeq - lesTemps[0] > 6000)
                    {
                        lesPics.RemoveAt(0);
                        lesTemps.RemoveAt(0);
                    }
                    lesPics.Add(data[j]);
                    lesTemps.Add(nSeq);

                }
                else
                {
                    lesPics.Add(data[j]);
                    lesTemps.Add(nSeq);
                }
                compteurPic += 1;
                compteurDistance = 0;
                ajoutPic = true;
            }
            else
            {
                CreerText(data[j] + "\n");
                compteurDistance += 1;
            }

        }
        // calcul freq cardiaque il faut au moin 1 seconde pour avoir une fréquence cardaique
        // methode pour calculer la moyenne de fréquenc cadiaque
        if (ajoutPic && lesPics.Count > 0 && nSeq > 100)
        {
            for (int t = 0; t < lesTemps.Count; t++)
            {
                //Debug.Log("temps: " + t + ": " + lesTemps[t] + "freq: " + freq);
                if (t > 0)
                {
                    freq += lesTemps[t] - lesTemps[t - 1];

                }
                // les 10 dernières freéquences cardiaques
                if (lesTemps.Count > 10 && t >= (lesTemps.Count - 10))
                {
                    moyen10RR += lesTemps[t] - lesTemps[t - 1];
                    // Debug.Log(lesTemps.Count - t);
                }
            }
            //on regarde le temps pour actuel en sec
            float tempsActuel = (nSeq / samplingRate);
            //Debug.Log("temps actuel: " + tempsActuel+"le round : "+ ((int)Math.Round(tempsActuel)));
            //produit en coix: x pic en y sec --> combien de pic/battemennt pour 60sec
            //(60*x pic)/y sec
            if (tempsActuel > 60)
            {
                tempsActuel = 60;
            }
            Debug.Log("pic par temps: " + (lesPics.Count * 60 / ((int)Math.Round(tempsActuel))) + "nbr de pic : " + lesPics.Count + " -- moyen des variabilité des rythme cartiaque : " + freq / lesTemps.Count + " la moyen v r-r 10s : " + moyen10RR / 10 + " --nseq actuel :  " + nSeq);
            if ((moyen10RR / 10)>80)
            {
                stresser = true;
            }
            else
            {
                stresser = false;
            }
            ajoutPic = false;
            freq = 0;
            moyen10RR = 0;
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

            // Reboot GUI.
            RebootGUI();
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
    public void RebootGUI()
    {
        ScanButton.interactable = true;
        ConnectButton.interactable = false;
        DisconnectButton.interactable = false;
        StartAcqButton.interactable = false;
        StopAcqButton.interactable = false;
        DeviceDropdown.interactable = false;
        SamplingRateDropdown.interactable = false;
        ResolutionDropdown.interactable = false;
        RedIntensityDropdown.interactable = false;
        InfraredIntensityDropdown.interactable = false;
    }
}