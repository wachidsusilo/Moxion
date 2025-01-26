import Time from "../values/Time.ts";

class TimeProfile {
    jerkDuration: Time
    constantAccelerationDuration: Time
    constantVelocityDuration: Time
    totalDuration: Time

    constructor(
        jerkDuration?: Time,
        constantAccelerationDuration?: Time,
        constantVelocityDuration?: Time,
        totalDuration?: Time
    ) {
        this.jerkDuration = jerkDuration ?? new Time()
        this.constantAccelerationDuration = constantAccelerationDuration ?? new Time()
        this.constantVelocityDuration = constantVelocityDuration ?? new Time()
        this.totalDuration = totalDuration ?? new Time()
    }

    static from(other: TimeProfile) {
        if (!other) {
            return new TimeProfile()
        }

        return new TimeProfile(
            Time.from(other.jerkDuration),
            Time.from(other.constantAccelerationDuration),
            Time.from(other.constantVelocityDuration),
            Time.from(other.totalDuration)
        )
    }
}

export default TimeProfile