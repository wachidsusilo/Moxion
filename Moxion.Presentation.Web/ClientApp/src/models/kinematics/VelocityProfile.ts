import Velocity from "../values/Velocity";

class VelocityProfile {
    positiveJerkMaxVelocity: Velocity
    constantAccelerationMaxVelocity: Velocity
    negativeJerkMaxVelocity: Velocity

    constructor(
        positiveJerkMaxVelocity?: Velocity,
        constantAccelerationMaxVelocity?: Velocity,
        negativeJerkMaxVelocity?: Velocity
    ) {
        this.positiveJerkMaxVelocity = positiveJerkMaxVelocity ?? new Velocity()
        this.constantAccelerationMaxVelocity = constantAccelerationMaxVelocity ?? new Velocity()
        this.negativeJerkMaxVelocity = negativeJerkMaxVelocity ?? new Velocity()
    }

    static from(other: VelocityProfile) {
        if (!other) {
            return new VelocityProfile()
        }

        return new VelocityProfile(
            Velocity.from(other.positiveJerkMaxVelocity),
            Velocity.from(other.constantAccelerationMaxVelocity),
            Velocity.from(other.negativeJerkMaxVelocity)
        )
    }
}

export default VelocityProfile