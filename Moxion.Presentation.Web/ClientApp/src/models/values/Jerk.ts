import PositionUnit from "../enumerations/PositionUnit";
import TimeUnit from "../enumerations/TimeUnit";
import JerkUnitInfo from "../kinematics/units/JerkUnitInfo.ts";

class Jerk {
    value: number
    positionUnit: PositionUnit
    timeUnit: TimeUnit

    constructor(value?: number, positionUnit?: PositionUnit, timeUnit?: TimeUnit) {
        this.value = value ?? 0
        this.positionUnit = positionUnit ?? 'Meter'
        this.timeUnit = timeUnit ?? 'Second'
    }

    getUnitInfo() {
        return new JerkUnitInfo(this.positionUnit, this.timeUnit)
    }

    static from(other: Jerk) {
        if (!other) {
            return new Jerk()
        }

        return new Jerk(other.value, other.positionUnit, other.timeUnit)
    }
}

export default Jerk