import Position from "../../models/values/Position";
import Velocity from "../../models/values/Velocity";
import Acceleration from "../../models/values/Acceleration";
import Jerk from "../../models/values/Jerk";
import TimeUnit from "../../models/enumerations/TimeUnit";
import MotionSimulationParam from "../../models/kinematics/MotionSimulationParam.ts";

class KinematicSimulateRequest {
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

    static fromParam(param: MotionSimulationParam): KinematicSimulateRequest {
        return new KinematicSimulateRequest(
            param.displacement,
            param.velocity,
            param.acceleration,
            param.jerk,
            param.timeIntervalUnit,
            param.dataCount
        )
    }
}

export default KinematicSimulateRequest