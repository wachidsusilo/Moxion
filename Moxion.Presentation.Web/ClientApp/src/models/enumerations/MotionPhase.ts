type MotionPhase =
    'None'
    | 'AccelerationWithPositiveJerk'
    | 'ConstantAcceleration'
    | 'AccelerationWithNegativeJerk'
    | 'ConstantVelocity'
    | 'DecelerationWithNegativeJerk'
    | 'ConstantDeceleration'
    | 'DecelerationWithPositiveJerk'

export default MotionPhase