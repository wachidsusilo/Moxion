import Position from "../values/Position";
import Velocity from "../values/Velocity";
import Acceleration from "../values/Acceleration";
import Jerk from "../values/Jerk";
import MotionProfileType from "../enumerations/MotionProfileType.ts";
import TimeProfile from "./TimeProfile.ts";
import PositionProfile from "./PositionProfile.ts";
import VelocityProfile from "./VelocityProfile.ts";
import AccelerationProfile from "./AccelerationProfile.ts";

class MotionProfile {
    profileType: MotionProfileType
    totalDisplacement: Position
    maxVelocity: Velocity
    maxAcceleration: Acceleration
    jerk: Jerk
    timeProfile: TimeProfile
    positionProfile: PositionProfile
    velocityProfile: VelocityProfile
    accelerationProfile: AccelerationProfile

    constructor(
        profileType?: MotionProfileType,
        totalDisplacement?: Position,
        maxVelocity?: Velocity,
        maxAcceleration?: Acceleration,
        jerk?: Jerk,
        timeProfile?: TimeProfile,
        positionProfile?: PositionProfile,
        velocityProfile?: VelocityProfile,
        accelerationProfile?: AccelerationProfile
    ) {
        this.profileType = profileType ?? 'None'
        this.totalDisplacement = totalDisplacement ?? new Position()
        this.maxVelocity = maxVelocity ?? new Velocity()
        this.maxAcceleration = maxAcceleration ?? new Acceleration()
        this.jerk = jerk ?? new Jerk()
        this.timeProfile = timeProfile ?? new TimeProfile()
        this.positionProfile = positionProfile ?? new PositionProfile()
        this.velocityProfile = velocityProfile ?? new VelocityProfile()
        this.accelerationProfile = accelerationProfile ?? new AccelerationProfile()
    }

    static from(other: MotionProfile | null) {
        if (!other) {
            return new MotionProfile()
        }

        return new MotionProfile(
            other.profileType,
            Position.from(other.totalDisplacement),
            Velocity.from(other.maxVelocity),
            Acceleration.from(other.maxAcceleration),
            Jerk.from(other.jerk),
            TimeProfile.from(other.timeProfile),
            PositionProfile.from(other.positionProfile),
            VelocityProfile.from(other.velocityProfile),
            AccelerationProfile.from(other.accelerationProfile)
        );
    }
}

export default MotionProfile