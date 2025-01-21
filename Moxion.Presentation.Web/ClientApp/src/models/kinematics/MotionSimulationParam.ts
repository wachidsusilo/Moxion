import Position from "../values/Position";
import Velocity from "../values/Velocity";
import Acceleration from "../values/Acceleration";
import Jerk from "../values/Jerk";
import TimeUnit from "../enumerations/TimeUnit";

class MotionSimulationParam {
    displacement: Position
    velocity: Velocity
    acceleration: Acceleration
    jerk: Jerk
    timeIntervalUnit: TimeUnit
    dataCount: number

    constructor(
        displacement?: Position,
        velocity?: Velocity,
        acceleration?: Acceleration,
        jerk?: Jerk,
        timeIntervalUnit?: TimeUnit,
        dataCount?: number
    ) {
        this.displacement = displacement ?? new Position()
        this.velocity = velocity ?? new Velocity()
        this.acceleration = acceleration ?? new Acceleration()
        this.jerk = jerk ?? new Jerk()
        this.timeIntervalUnit = timeIntervalUnit ?? 'Second'
        this.dataCount = dataCount ?? 0
    }
}

export default MotionSimulationParam