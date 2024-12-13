using System.Collections.Generic;

namespace VitsehLand.Scripts.UI.MonitorGarden
{
    public class GrowingSlotResourceManager : SlotManager
    {
        public List<GrowingSlotResourceState> growingSlotResourcesManager = new List<GrowingSlotResourceState>();

        public void FillResourceSlot(int i)
        {
            growingSlotResourcesManager[i].ChangeFillState(true);
        }

        public void MakeResourceSlotEmpty(int i)
        {
            growingSlotResourcesManager[i].ChangeFillState(false);
        }
    }
}