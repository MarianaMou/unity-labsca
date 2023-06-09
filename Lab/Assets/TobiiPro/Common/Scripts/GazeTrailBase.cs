 //-----------------------------------------------------------------------
// Copyright © 2019 Tobii Pro AB. All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;
// a moi
using UnityEngine.UI;

namespace Tobii.Research.Unity
{
    public class GazeTrailBase : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The color of the particle")]
        private Color _color = Color.green;

        [SerializeField]
        [Range(0, 1000)]
        [Tooltip("The number of particles to allocate. Use zero to use only the last hit object.")]
        private int _particleCount = 100;

        [SerializeField]
        [Range(0.005f, 0.2f)]
        [Tooltip("The size of the particle.")]
        private float _particleSize = 0.05f;

        [SerializeField]
        [Tooltip("Turn gaze trail on or off.")]
        public bool _on = true;

        /// <summary>
        /// Turn gaze trail on or off.
        /// </summary>
        public bool On
        {
            get
            {
                return _on;
            }

            set
            {
                _on = value;
                OnSwitch();
            }
        }

        /// <summary>
        /// Set particle count between 1 and 1000.
        /// </summary>
        public int ParticleCount
        {
            get
            {
                return _particleCount;
            }

            set
            {
                if (value < 0 || value > 1000)
                {
                    return;
                }

                _particleCount = value;
                CheckCount();
            }
        }

        public Color ParticleColor
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }

        /// <summary>
        /// Get the latest hit object.
        /// </summary>
        public Transform LatestHitObject
        {
            get
            {
                return _latestHitObject;
            }
        }
        //a moi
        public Camera j2;
        private float tempsRegardExit=0;
        private float tempsRegard=0;
        public  Slider progressMonitor;
        private bool monitorView = false;
        private int enigmeActuel = 7;
        public GameObject exitMonitor;
        public GameObject[] enigmesSortie;
        public Slider[] sliderEnigmes;
        public Slider[] ExitSliderEnigmes;
        public Slider progessExitMonitor;

        private bool _lastOn;
        private int _lastParticleCount;
        private int _particleIndex;
        private ParticleSystem.Particle[] _particles;
        private ParticleSystem _particleSystem;
        private bool _particlesDirty;
        private Transform _latestHitObject;
        private bool _removeParticlesWhileCalibrating;

        protected virtual bool HasEyeTracker { get; set; }
        protected virtual bool CalibrationInProgress { get; set; }

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
            _lastParticleCount = _particleCount;
            _particles = new ParticleSystem.Particle[_particleCount];
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (_particlesDirty)
            {
                _particleSystem.SetParticles(_particles, _particles.Length);
                _particlesDirty = false;
            }

            CheckCount();

            OnSwitch();

            if (CalibrationInProgress)
            {
                // Don't do anything if we are calibrating.

                if (_removeParticlesWhileCalibrating)
                {
                    RemoveParticles();
                    _removeParticlesWhileCalibrating = false;
                }

                return;
            }

            // Reset the flag when no longer calibrating.
            _removeParticlesWhileCalibrating = true;


            if (HasEyeTracker && _on)
            {
                Ray ray;
                var valid = GetRay(out ray);
                if (valid)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        PlaceParticle(hit.point, _color, _particleSize);
                        _latestHitObject = hit.transform;
                        //Debug.Log(_latestHitObject.name);
                        // etat de base
                        if (!monitorView && enigmeActuel==7)
                        {
                            exitMonitor.transform.localScale = new Vector3(0f, 0f, 0f);
                            progressMonitor.transform.localScale = new Vector3(0f,0f,0f);

                            //il regarde le moniteur
                            if (_latestHitObject.name == "Monitor")
                            {

                                progressMonitor.transform.localScale = new Vector3(0.8f, 0.8f, 0f);
                                progressMonitor.value += Time.deltaTime;
                                //Debug.Log("temps à regader le moniteur" + progressMonitor.value+ "max" + progressMonitor.maxValue);

                                //change de vue : vue moniteur
                                if (progressMonitor.value >= progressMonitor.maxValue)
                                {
                                    j2.transform.localEulerAngles = new Vector3(17f, 37f, 0f);
                                    j2.fieldOfView = 50f;
                                    monitorView = true;
                                    exitMonitor.transform.localScale = new Vector3(0.75f, 1.1f, 0.1f);
                                    progressMonitor.transform.localScale = new Vector3(0f,0f,0f);
                                }

                                for(int e=0; e<sliderEnigmes.Length; e++)
                                {
                                    sliderEnigmes[e].value = 0;
                                    ExitSliderEnigmes[e].value = 0;
                                    sliderEnigmes[e].transform.localScale = new Vector3(0f, 0f, 0f);
                                }


                            }
                            //il regarde page 1
                            if (_latestHitObject.name == "PageE1")
                            {
                                Enigma(0, new Vector3(-1.1f, 0.2f, -3.25f));
                               // sliderEnigmes[0].value += Time.deltaTime;
                               /* if (sliderEnigmes[0].value>=sliderEnigmes[0].maxValue)
                                {
                                    j2.transform.localPosition = new Vector3(-1.2f, 0.2f, -3.25f);
                                    j2.transform.rotation = Quaternion.Euler(67f, 0.1f, 0f); //   Rotate(new Vector3(67f, 0.1f, 0f),20f*Time.deltaTime);
                                    enigmesSortie[0].transform.localPosition = new Vector3(1f, 1f, 0f);
                                    j2.fieldOfView = 35f;
                                    enigmeActuel = 1;
                                    monitorView = false;
                                }*/
                                
                            }
                            if (_latestHitObject.name == "PageE2")
                            {
                                Enigma(1, new Vector3(-0.4f, 0.2f, -3.35f));
                                /*sliderEnigmes[1].value += Time.deltaTime;

                                    j2.transform.localPosition = new Vector3(-0.66f, 0.3f, -3.43f);
                                j2.transform.rotation = Quaternion.Euler(67f, 0.1f, 0f); //   Rotate(new Vector3(67f, 0.1f, 0f),20f*Time.deltaTime);
                                j2.fieldOfView = 35f;
                                monitorView = false;*/


                            }
                            if (_latestHitObject.name == "PageE3")
                            {
                                Enigma(2, new Vector3(0.3f, 0.2f, -2.75f));
                                /*sliderEnigmes[2].value += Time.deltaTime;
                                j2.transform.localPosition = new Vector3(-0.24f, 0.3f, -3.15f);
                                j2.transform.rotation = Quaternion.Euler(67f, 0.1f, 0f); //   Rotate(new Vector3(67f, 0.1f, 0f),20f*Time.deltaTime);
                                j2.fieldOfView = 35f;
                                monitorView = false;*/
                            }
                            if (_latestHitObject.name == "PageE4")
                            {
                                Enigma(3, new Vector3(0.87f, 0.2f, -3.5f));
                                /*sliderEnigmes[3].value += Time.deltaTime;
                                j2.transform.localPosition = new Vector3(0.2f, 0.3f, -3.5f);
                                j2.transform.rotation = Quaternion.Euler(67f, 0.1f, 0f); //   Rotate(new Vector3(67f, 0.1f, 0f),20f*Time.deltaTime);
                                j2.fieldOfView = 35f;
                                monitorView = false;*/
                            }
                            if (_latestHitObject.name == "PageE5")
                            {
                                Enigma(4, new Vector3(0.2f, 0.2f, -3.73f));
                                /*sliderEnigmes[4].value += Time.deltaTime;
                                j2.transform.localPosition = new Vector3(-0.2f, 0.3f, -3.73f);
                                j2.transform.rotation = Quaternion.Euler(67f, 0.1f, 0f); //   Rotate(new Vector3(67f, 0.1f, 0f),20f*Time.deltaTime);
                                j2.fieldOfView = 35f;
                                monitorView = false;*/
                            }
                            if (_latestHitObject.name == "PageE6")
                            {
                                Enigma(5, new Vector3(-0.33f, 0.2f, -2.75f));
                                /*sliderEnigmes[5].value += Time.deltaTime;
                                j2.transform.localPosition = new Vector3(-0.7f, 0.3f, -2.85f);
                                j2.transform.rotation = Quaternion.Euler(67f, 0.1f, 0f); //   Rotate(new Vector3(67f, 0.1f, 0f),20f*Time.deltaTime);
                                j2.fieldOfView = 35f;
                                monitorView = false;*/
                            }
                            if(_latestHitObject.name== "bureau_j2" || _latestHitObject.name == "Keyboard" || _latestHitObject.name == "Mouse" || _latestHitObject.name == "Plane_j2" || _latestHitObject.name == "Book1" || _latestHitObject.name == "Bookk2" || _latestHitObject.name == "Scroll" || _latestHitObject.name == "MurA" || _latestHitObject.name == "MurB" || _latestHitObject.name == "MurC" || _latestHitObject.name == "plateau")
                            {
                                
                                progressMonitor.value = 0f;
                                for(int e=0; e<sliderEnigmes.Length; e++)
                                {
                                    sliderEnigmes[e].value = 0;
                                    sliderEnigmes[e].transform.localScale = new Vector3(0f, 0f, 0f);
                                }
                            }

                        }
                        //je suis en vue moniteur
                        else if (monitorView)
                        {
                            if (_latestHitObject.name == "ExitMonitor")
                            {
                                tempsRegardExit += Time.deltaTime;
                                progessExitMonitor.value = tempsRegardExit;
                                if (tempsRegardExit >= progessExitMonitor.maxValue)
                                {
                                    j2.transform.localEulerAngles = new Vector3(17f, 0f, 0f);
                                    j2.fieldOfView = 70f;
                                    monitorView = false;

                                }

                            }
                            else
                            {
                                tempsRegardExit = 0f;
                                progessExitMonitor.value = 0f;
                            }
                            tempsRegard = 0f;
                        }
                        // je regarde une enigme
                        else if (enigmeActuel<7)
                        {
                            sortieEnigme();
                            
                        }
                       
                        //Debug.Log("gazeRarilCase je touche"+_latestHitObject);
                       
                        
                       
                    }
                    else
                    {
                        progressMonitor.transform.localScale = new Vector3(0f, 0f, 0f);
                        _latestHitObject = null;
                        tempsRegard = 0f;
                        tempsRegardExit = 0f;
                    }
                }
            }
        }

        public void Enigma(int e, Vector3 pos)
        {
            if (enigmeActuel==7){
                sliderEnigmes[e].transform.localScale = new Vector3(0.001f, 0.001f, 0f);
                sliderEnigmes[e].value += Time.deltaTime;
            }

            if (sliderEnigmes[e].value >= sliderEnigmes[e].maxValue)
            {
                j2.transform.localPosition = pos;
                j2.transform.rotation = Quaternion.Euler(67f, 0.1f, 0f); //   Rotate(new Vector3(67f, 0.1f, 0f),20f*Time.deltaTime);
                enigmesSortie[e].transform.localPosition = new Vector3(1f, 1f, 0f);
                j2.fieldOfView = 50f;
                enigmeActuel = e;
                monitorView = false;
            }

        }
        public void sortieEnigme()
        {
            sliderEnigmes[enigmeActuel].transform.localScale = new Vector3(0f, 0f, 0f);
            if (_latestHitObject.name == "ExitE"+(enigmeActuel+1).ToString())
            {
                ExitSliderEnigmes[enigmeActuel].value += Time.deltaTime;
                if (ExitSliderEnigmes[enigmeActuel].value >= ExitSliderEnigmes[enigmeActuel].maxValue)
                {
                    //Debug.Log("je suis sortie?");
                    j2.transform.localEulerAngles = new Vector3(17f, 0f, 0f);
                    j2.transform.localPosition = new Vector3(0.1f, 0.45f, -4.5f);
                    enigmesSortie[enigmeActuel].transform.localPosition = new Vector3(1f, -10f, 0f);
                    ExitSliderEnigmes[enigmeActuel].value = 0;
                    j2.fieldOfView = 70f;
                    enigmeActuel = 7;
                }
            }
        }

        private void CheckCount()
        {
            if (_lastParticleCount != _particleCount)
            {
                RemoveParticles();
                _particleIndex = 0;
                _particles = new ParticleSystem.Particle[_particleCount];
                _lastParticleCount = _particleCount;
            }
        }

        private void OnSwitch()
        {
            if (_lastOn && !_on)
            {
                // Switch off.
                RemoveParticles();
                _lastOn = false;
            }
            else if (!_lastOn && _on)
            {
                // Switch on.
                _lastOn = true;
            }
        }

        private void RemoveParticles()
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                PlaceParticle(Vector3.zero, Color.white, 0);
            }
        }

        private void PlaceParticle(Vector3 pos, Color color, float size)
        {
            if (_particleCount < 1)
            {
                return;
            }

            var particle = _particles[_particleIndex];
            particle.position = pos;
            //Debug.Log("GaTraiBas217 la pos" + pos);
            particle.startColor = color;
            particle.startSize = size;
            _particles[_particleIndex] = particle;
            _particleIndex = (_particleIndex + 1) % _particles.Length;
            _particlesDirty = true;
        }

        protected virtual bool GetRay(out Ray ray)
        {
            ray = default(Ray);
            return false;
        }
    }
}