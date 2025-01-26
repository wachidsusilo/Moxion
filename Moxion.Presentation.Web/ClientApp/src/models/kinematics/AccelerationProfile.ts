import Acceleration from "../values/Acceleration";

class AccelerationProfile {
    positiveJerkMaxAcceleration: Acceleration

    constructor(positiveJerkMaxAcceleration?: Acceleration) {
        this.positiveJerkMaxAcceleration = positiveJerkMaxAcceleration ?? new Acceleration()
    }

    static from(other: AccelerationProfile) {
        if (!other) {
            return new AccelerationProfile()
        }

        return new AccelerationProfile(
            Acceleration.from(other.positiveJerkMaxAcceleration)
        )
    }
}

export default AccelerationProfile