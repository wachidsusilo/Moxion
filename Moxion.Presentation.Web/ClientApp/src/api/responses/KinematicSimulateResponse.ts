import ErrorCode from "../../models/enumerations/ErrorCode.ts";
import MotionProfile from "../../models/kinematics/MotionProfile.ts";
import MotionData from "../../models/kinematics/MotionData.ts";
import KinematicUnitInfo from "../../models/kinematics/units/KinematicUnitInfo.ts";

class KinematicSimulateResponse {
    errorCode: ErrorCode
    profile: MotionProfile | null
    data: MotionData[] | null
    unitInfo: KinematicUnitInfo | null

    constructor(
        errorCode?: ErrorCode,
        profile?: MotionProfile | null,
        data?: MotionData[] | null,
        unitInfo?: KinematicUnitInfo | null
    ) {
        this.errorCode = errorCode ?? 'NoError'
        this.profile = profile ?? null
        this.data = data ?? null
        this.unitInfo = unitInfo ?? null
    }
}

export default KinematicSimulateResponse