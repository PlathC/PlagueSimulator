using UnityEngine;

namespace Model.Agents
{
    public class AgentProximity : MonoBehaviour
    {
        private CitizenBody m_parent;

        public void Start()
        {
            m_parent = transform.parent.gameObject.GetComponent<CitizenBody>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var otherBody = other.gameObject.GetComponentInParent<CitizenBody>();
            if (!otherBody) return;

            if (other.GetType() == typeof(BoxCollider))
            {
                m_parent.NotifyAgentProximityEnter(otherBody);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            var otherBody = other.gameObject.GetComponentInParent<CitizenBody>();
            if (!otherBody) return;

            if (other.GetType() == typeof(BoxCollider))
            {
                m_parent.NotifyAgentProximityExit(otherBody);
            }
        }
    }
}
