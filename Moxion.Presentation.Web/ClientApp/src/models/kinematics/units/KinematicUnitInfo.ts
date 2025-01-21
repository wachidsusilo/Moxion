import PositionUnitInfo from "./PositionUnitInfo";
import VelocityUnitInfo from "./VelocityUnitInfo";
import JerkUnitInfo from "./JerkUnitInfo";
import AccelerationUnitInfo from "./AccelerationUnitInfo";

class KinematicUnitInfo {
    displacement: PositionUnitInfo = new PositionUnitInfo('None')
    velocity: VelocityUnitInfo = new VelocityUnitInfo('None', 'None')
    acceleration: AccelerationUnitInfo = new AccelerationUnitInfo('None', 'None')
    jerk: JerkUnitInfo = new JerkUnitInfo('None', 'None')
}

export default KinematicUnitInfo