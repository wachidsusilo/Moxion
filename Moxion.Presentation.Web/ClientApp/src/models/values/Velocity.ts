import PositionUnit from "../enumerations/PositionUnit";
import TimeUnit from "../enumerations/TimeUnit";
import VelocityUnitInfo from "../kinematics/units/VelocityUnitInfo.ts";

class Velocity {
    value: number
    positionUnit: PositionUnit
    timeUnit: TimeUnit

    constructor(value?: number, positionUnit?: PositionUnit, timeUnit?: TimeUnit) {
        this.value = value ?? 0
        this.positionUnit = positionUnit ?? 'Meter'
        this.timeUnit = timeUnit ?? 'Second'
    }

    getUnitInfo() {
        return new VelocityUnitInfo(this.positionUnit, this.timeUnit)
    }
}

export default Velocity