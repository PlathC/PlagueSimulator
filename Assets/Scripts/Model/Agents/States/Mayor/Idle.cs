using Model.Environment;
using UnityEngine;

namespace Model.Agents.States.Mayor
{
    public class Idle : MayorState
    {
        public Idle(AgentEnvironment environment, Agents.Mayor mayor) : base(environment, mayor)
        {
        }
        
        public override IState Action()
        {
            //TODO: include death number
            if (Random.Range(0, 10) > 5)
                    return new TimeOutside(m_environment, m_mayor, m_environment.LastGrowthRate);

            return new SocialDistancing(m_environment, m_mayor, m_environment.LastGrowthRate);
            
        }
    }
}