import MotionChart from "../components/charts/MotionChart.tsx";
import Header from "../components/Header.tsx";
import useMotionSimulation from "../hooks/UseMotionSimulation.tsx";
import {useEffect, useRef} from "react";

function MotionSimulationPage() {
    const displacementRef = useRef<HTMLInputElement | null>(null)
    const velocityRef = useRef<HTMLInputElement | null>(null)
    const accelerationRef = useRef<HTMLInputElement | null>(null)
    const jerkRef = useRef<HTMLInputElement | null>(null)
    const dataCountRef = useRef<HTMLInputElement | null>(null)

    const {param, setParam, data, simulate, isLoading, error} = useMotionSimulation()

    useEffect(() => {
        if (!displacementRef.current) {
            return;
        }

        displacementRef.current!.value = param.displacement.value.toString()
        velocityRef.current!.value = param.velocity.value.toString()
        accelerationRef.current!.value = param.acceleration.value.toString()
        jerkRef.current!.value = param.jerk.value.toString()
        dataCountRef.current!.value = param.dataCount.toString()
    }, []);

    function onClick() {
        param.displacement.value = parseFloat(displacementRef.current!.value) ?? 0
        param.velocity.value = parseFloat(velocityRef.current!.value) ?? 0
        param.acceleration.value = parseFloat(accelerationRef.current!.value) ?? 0
        param.jerk.value = parseFloat(jerkRef.current!.value) ?? 0
        param.dataCount = parseFloat(dataCountRef.current!.value) ?? 0

        setParam(param)
        simulate()
    }

    return (
        <div className="w-full h-full flex flex-col">
            <Header/>
            <div className="w-full h-full flex">
                <div className="w-[400px] h-[calc(100vh-92px)] flex flex-col shrink-0 hairline-r overflow-hidden">
                    <div className="h-[330px] px-8 flex flex-col gap-4 overflow-hidden">
                        <div className="mt-8 motion-simulation-input-container">
                            <div className="motion-simulation-input-label">Displacement</div>
                            <input ref={displacementRef} className="motion-simulation-input-value" type="number"/>
                            <div>{param.displacement.getUnitInfo().toString()}</div>
                        </div>
                        <div className="motion-simulation-input-container">
                            <div className="motion-simulation-input-label">Velocity</div>
                            <input ref={velocityRef} className="motion-simulation-input-value" type="number"/>
                            <div>{param.velocity.getUnitInfo().toString()}</div>
                        </div>
                        <div className="motion-simulation-input-container">
                            <div className="motion-simulation-input-label">Acceleration</div>
                            <input ref={accelerationRef} className="motion-simulation-input-value" type="number"/>
                            <div>{param.acceleration.getUnitInfo().toString()}</div>
                        </div>
                        <div className="motion-simulation-input-container">
                            <div className="motion-simulation-input-label">Jerk</div>
                            <input ref={jerkRef} className="motion-simulation-input-value" type="number"/>
                            <div>{param.jerk.getUnitInfo().toString()}</div>
                        </div>
                        <div className="motion-simulation-input-container">
                            <div className="motion-simulation-input-label">Data Count</div>
                            <input ref={dataCountRef} className="motion-simulation-input-value" type="number"/>
                        </div>
                        <div className="w-full mt-8 flex items-center justify-center">
                            <button className="w-[200px] h-[35px] button bg-blue-500/50"
                                    disabled={isLoading}
                                    onClick={onClick}>
                                {
                                    isLoading
                                        ? <div className="progress-25"/>
                                        : "Simulate"
                                }
                            </button>
                        </div>
                    </div>
                    <div className="h-8 mx-8 hairline-b"/>
                    <div className="h-[calc(100%-364px)] overflow-auto">
                        <div className="py-8 px-8 flex flex-col items-start gap-2">
                            <div className="motion-simulation-result-title mt-0">
                                Simulation Result
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Error</div>
                                <div
                                    className={`motion-simulation-result-value ${error === 'NoError' ? "text-no-error" : "text-error"}`}>
                                    {error}
                                </div>
                            </div>
                            <div className="motion-simulation-result-title">
                                Motion Profile
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Profile Type</div>
                                <div className="motion-simulation-result-value-string">{data.profile.profileType}</div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Total Displacement</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.totalDisplacement.value.toFixed(2)}
                                </div>
                                <div
                                    className="motion-simulation-result-unit">{data.profile.totalDisplacement.getUnitInfo().toString()}</div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Max. Velocity</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.maxVelocity.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.maxVelocity.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Max. Acceleration</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.maxAcceleration.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.maxAcceleration.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Jerk</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.jerk.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.jerk.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="motion-simulation-result-title">
                                Time Profile
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Jerk</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.timeProfile.jerkDuration.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.timeProfile.jerkDuration.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Const. Acceleration</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.timeProfile.constantAccelerationDuration.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.timeProfile.constantAccelerationDuration.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Const. Velocity</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.timeProfile.constantVelocityDuration.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.timeProfile.constantVelocityDuration.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Total</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.timeProfile.totalDuration.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.timeProfile.totalDuration.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="motion-simulation-result-title">
                                Displacement Profile
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Positive Jerk</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.positionProfile.positiveJerkDisplacement.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.positionProfile.positiveJerkDisplacement.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Negative Jerk</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.positionProfile.negativeJerkDisplacement.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.positionProfile.negativeJerkDisplacement.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Const. Acceleration</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.positionProfile.constantAccelerationDisplacement.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.positionProfile.constantAccelerationDisplacement.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Const. Velocity</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.positionProfile.constantVelocityDisplacement.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.positionProfile.constantVelocityDisplacement.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="motion-simulation-result-title">
                                Velocity Profile
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Positive Jerk</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.velocityProfile.positiveJerkMaxVelocity.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.velocityProfile.positiveJerkMaxVelocity.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Const. Acceleration</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.velocityProfile.constantAccelerationMaxVelocity.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.velocityProfile.constantAccelerationMaxVelocity.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Negative Jerk</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.velocityProfile.negativeJerkMaxVelocity.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.velocityProfile.negativeJerkMaxVelocity.getUnitInfo().toString()}
                                </div>
                            </div>
                            <div className="motion-simulation-result-title">
                                Acceleration Profile
                            </div>
                            <div className="flex">
                                <div className="motion-simulation-result-label">Positive Jerk</div>
                                <div className="motion-simulation-result-value-number">
                                    {data.profile.accelerationProfile.positiveJerkMaxAcceleration.value.toFixed(2)}
                                </div>
                                <div className="motion-simulation-result-unit">
                                    {data.profile.accelerationProfile.positiveJerkMaxAcceleration.getUnitInfo().toString()}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <MotionChart data={data}/>
            </div>
        </div>
    )
}

export default MotionSimulationPage