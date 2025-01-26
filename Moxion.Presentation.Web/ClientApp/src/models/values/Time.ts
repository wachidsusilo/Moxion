import TimeUnit from "../enumerations/TimeUnit";
import TimeUnitInfo from "../kinematics/units/TimeUnitInfo.ts";

class Time {
    value: number
    unit: TimeUnit

    constructor(value?: number, unit?: TimeUnit) {
        this.value = value ?? 0
        this.unit = unit ?? 'Second'
    }

    getUnitInfo() {
        return new TimeUnitInfo(this.unit)
    }

    static from(other: Time) {
        if (!other) {
            return new Time()
        }

        return new Time(other.value, other.unit)
    }
}

export default Time