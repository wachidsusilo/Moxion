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
}

export default Jerk