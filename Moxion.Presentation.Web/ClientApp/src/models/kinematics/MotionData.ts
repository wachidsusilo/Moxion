import MotionPhase from "../enumerations/MotionPhase.ts";

class MotionData {
    time: number
    position: number
    velocity: number
    acceleration: number
    jerk: number
    phase: MotionPhase

    constructor(
        time?: number,
        position?: number,
        velocity?: number,
        acceleration?: number,
        jerk?: number,
        phase?: MotionPhase
    ) {
        this.time = time ?? 0
        this.position = position ?? 0
        this.velocity = velocity ?? 0
        this.acceleration = acceleration ?? 0
        this.jerk = jerk ?? 0
        this.phase = phase ?? 'None'
    }
}

export default MotionData