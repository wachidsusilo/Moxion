import PositionUnitInfo from "./PositionUnitInfo";
import VelocityUnitInfo from "./VelocityUnitInfo";
import JerkUnitInfo from "./JerkUnitInfo";
import AccelerationUnitInfo from "./AccelerationUnitInfo";

class KinematicUnitInfo {
    displacement: PositionUnitInfo
    velocity: VelocityUnitInfo
    acceleration: AccelerationUnitInfo
    jerk: JerkUnitInfo

    constructor(
        displacement?: PositionUnitInfo,
        velocity?: VelocityUnitInfo,
        acceleration?: AccelerationUnitInfo,
        jerk?: JerkUnitInfo
    ) {
        this.displacement = displacement ?? new PositionUnitInfo('None');
        this.velocity = velocity ?? new VelocityUnitInfo('None', 'None');
        this.acceleration = acceleration ?? new AccelerationUnitInfo('None', 'None');
        this.jerk = jerk ?? new JerkUnitInfo('None', 'None');
    }

    static from(other: KinematicUnitInfo | null) {
        if (!other) {
            return new KinematicUnitInfo()
        }

        return new KinematicUnitInfo(
            PositionUnitInfo.from(other.displacement),
            VelocityUnitInfo.from(other.velocity),
            AccelerationUnitInfo.from(other.acceleration),
            JerkUnitInfo.from(other.jerk),
        )
    }
}

export default KinematicUnitInfo