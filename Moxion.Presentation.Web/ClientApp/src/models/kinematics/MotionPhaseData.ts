import MotionPhase from "../enumerations/MotionPhase.ts";

class MotionPhaseData{
    time: number
    phase: MotionPhase

    constructor(time: number, phase: MotionPhase) {
        this.time = time
        this.phase = phase
    }
}

export default MotionPhaseData