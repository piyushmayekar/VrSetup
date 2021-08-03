using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class ChillPlateTask : Task
    {
        [SerializeField] int plateCount = -1;
        [SerializeField] List<XRSocketInteractor> plateSockets;
        [SerializeField] List<GameObject> placedPlates = new List<GameObject>();
        [SerializeField] List<MeshRenderer> socketRenderers=new List<MeshRenderer>();
        [SerializeField] Material chillPlateMat;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            plateSockets.ForEach(socket => socketRenderers.Add(socket.GetComponent<MeshRenderer>()));
            Step();
        }

        void Step()
        {
            plateCount++;
            if (plateCount < plateSockets.Count)
            {
                plateSockets[plateCount].socketActive = true;
                socketRenderers[plateCount].enabled = true;
            }
            else
            {
                OnTaskCompleted();
            }
        }

        public void OnChillPlateEnterSocket(SelectEnterEventArgs args)
        {
            if (plateSockets[plateCount] == args.interactor)
            {
                //Turn off chill plate grabbable
                GameObject _plate = args.interactable.gameObject;
                _plate.SetActive(false);
                placedPlates.Add(_plate);
                _plate.transform.SetPositionAndRotation(plateSockets[plateCount].transform.position, plateSockets[plateCount].transform.rotation);

                //Mesh renderer change material
                socketRenderers[plateCount].material = chillPlateMat;

                //Socket collider non trigger
                plateSockets[plateCount].socketActive = false;
                plateSockets[plateCount].GetComponent<Collider>().isTrigger = false;

                Step();
            }
        }

        public void TurnOnGrabbableChillPlates()
        {
            plateSockets.ForEach(socket => socket.gameObject.SetActive(false));
            placedPlates.ForEach(plate => plate.SetActive(true));
        }
    }
}