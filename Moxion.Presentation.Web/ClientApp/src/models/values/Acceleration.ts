import PositionUnit from "../enumerations/PositionUnit";
import TimeUnit from "../enumerations/TimeUnit";
import AccelerationUnitInfo from "../kinematics/units/AccelerationUnitInfo.ts";

class Acceleration {
    value: number
    positionUnit: PositionUnit
    timeUnit: TimeUnit

    constructor(value?: number, positionUnit?: PositionUnit, timeUnit?: TimeUnit) {
        this.value = value ?? 0
        this.positionUnit = positionUnit ?? 'Meter'
        this.timeUnit = timeUnit ?? 'Second'
    }

    getUnitInfo() {
        return new AccelerationUnitInfo(this.positionUnit, this.timeUnit)
    }

    static from(other: Acceleration) {
        if (!other) {
            return new Acceleration()
        }

        return new Acceleration(other.value, other.positionUnit, other.timeUnit)
    }
}

export default Acceleration