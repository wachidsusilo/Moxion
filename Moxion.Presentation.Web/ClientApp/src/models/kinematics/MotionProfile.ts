import Time from "../values/Time";
import Position from "../values/Position";
import Velocity from "../values/Velocity";
import Acceleration from "../values/Acceleration";
import Jerk from "../values/Jerk";
import MotionProfileType from "../enumerations/MotionProfileType.ts";

class MotionProfile {
    profileType: MotionProfileType
    displacement: Position
    velocity: Velocity
    acceleration: Acceleration
    jerk: Jerk
    JerkDuration: Time
    AccelerationDuration: Time
    SteadyMotionDuration: Time
    TotalDuration: Time

    constructor(
        profileType?: MotionProfileType,
        displacement?: Position,
        velocity?: Velocity,
        acceleration?: Acceleration,
        jerk?: Jerk,
        JerkDuration?: Time,
        AccelerationDuration?: Time,
        SteadyMotionDuration?: Time,
        TotalDuration?: Time
    ) {
        this.profileType = profileType ?? 'None'
        this.displacement = displacement ?? new Position()
        this.velocity = velocity ?? new Velocity()
        this.acceleration = acceleration ?? new Acceleration()
        this.jerk = jerk ?? new Jerk()
        this.JerkDuration = JerkDuration ?? new Time()
        this.AccelerationDuration = AccelerationDuration ?? new Time()
        this.SteadyMotionDuration = SteadyMotionDuration ?? new Time()
        this.TotalDuration = TotalDuration ?? new Time()
    }
}

export default MotionProfile