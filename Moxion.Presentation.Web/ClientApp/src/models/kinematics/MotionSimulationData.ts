import MotionProfile from "./MotionProfile.ts";
import Point from "../graphics/Point.ts";
import MotionPhaseData from "./MotionPhaseData.ts";
import KinematicUnitInfo from "./units/KinematicUnitInfo.ts";

class MotionSimulationData {
    profile: MotionProfile
    positionData: Point[]
    velocityData: Point[]
    accelerationData: Point[]
    jerkData: Point[]
    phaseData: MotionPhaseData[]
    unitInfo: KinematicUnitInfo

    constructor(
        profile?: MotionProfile,
        positionData?: Point[],
        velocityData?: Point[],
        accelerationData?: Point[],
        jerkData?: Point[],
        phaseData?: MotionPhaseData[],
        unitInfo?: KinematicUnitInfo
    ) {
        this.profile = profile ?? new MotionProfile();
        this.positionData = positionData ?? [];
        this.velocityData = velocityData ?? [];
        this.accelerationData = accelerationData ?? [];
        this.jerkData = jerkData ?? [];
        this.phaseData = phaseData ?? [];
        this.unitInfo = unitInfo ?? new KinematicUnitInfo();
    }
}

export default MotionSimulationData