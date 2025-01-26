import Position from "../values/Position";

class PositionProfile {
    positiveJerkDisplacement: Position
    negativeJerkDisplacement: Position
    constantAccelerationDisplacement: Position
    constantVelocityDisplacement: Position

    constructor(
        positiveJerkDisplacement?: Position,
        negativeJerkDisplacement?: Position,
        constantAccelerationDisplacement?: Position,
        constantVelocityDisplacement?: Position
    ) {
        this.positiveJerkDisplacement = positiveJerkDisplacement ?? new Position()
        this.negativeJerkDisplacement = negativeJerkDisplacement ?? new Position()
        this.constantAccelerationDisplacement = constantAccelerationDisplacement ?? new Position()
        this.constantVelocityDisplacement = constantVelocityDisplacement ?? new Position()
    }

    static from(other: PositionProfile) {
        if (!other) {
            return new PositionProfile()
        }

        return new PositionProfile(
            Position.from(other.positiveJerkDisplacement),
            Position.from(other.negativeJerkDisplacement),
            Position.from(other.constantAccelerationDisplacement),
            Position.from(other.constantVelocityDisplacement)
        )
    }
}

export default PositionProfile