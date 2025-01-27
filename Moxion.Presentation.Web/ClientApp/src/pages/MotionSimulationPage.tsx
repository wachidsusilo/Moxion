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
                <div className="w-[400px] px-8 flex flex-col shrink-0 gap-4 shadow-[1px_0px_0px_0px_rgba(255_255_255_/_0.1)]">
                    <div className="mt-8 flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Displacement</div>
                        <input ref={displacementRef}
                               className="h-[30px] w-[100px] px-2 outline outline-1 outline-green-500" type="number"/>
                        <div>{param.displacement.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Velocity</div>
                        <input ref={velocityRef} className="h-[30px] w-[100px] px-2 outline outline-1 outline-green-500"
                               type="number"/>
                        <div>{param.velocity.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Acceleration</div>
                        <input ref={accelerationRef}
                               className="h-[30px] w-[100px] px-2 outline outline-1 outline-green-500" type="number"/>
                        <div>{param.acceleration.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Jerk</div>
                        <input ref={jerkRef} className="h-[30px] w-[100px] px-2 outline outline-1 outline-green-500"
                               type="number"/>
                        <div>{param.jerk.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Data Count</div>
                        <input ref={dataCountRef}
                               className="h-[30px] w-[100px] px-2 outline outline-1 outline-green-500"
                               type="number"/>
                    </div>
                    <div className="w-full mt-8 flex items-center justify-center">
                        <button className="w-[200px] h-[35px] flex items-center justify-center bg-blue-500/50"
                                disabled={isLoading}
                                onClick={onClick}>
                            Simulate
                        </button>
                    </div>
                    <div className="mt-4 pt-8 flex flex-col gap-2 shadow-[0px_-1px_0px_0px_rgba(255_255_255_/_0.1)]">
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Error</div>
                            <div className={`px-2 flex items-center 
                            ${error === 'NoError' ? "text-green-400" : "text-red-400"}`}>
                                {error}
                            </div>
                        </div>
                        <div className="flex overflow-hidden">
                            <div className="w-[160px] shrink-0 text-sm text-nowrap">Motion Profile</div>
                            <div className="px-2 flex items-center text-blue-400">{data.profile.profileType}</div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Total Displacement</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.totalDisplacement.value.toFixed(2)}
                            </div>
                            <div
                                className="flex items-center text-sm">{data.profile.totalDisplacement.getUnitInfo().toString()}</div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Max. Velocity</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.maxVelocity.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.maxVelocity.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Max. Acceleration</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.maxAcceleration.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.maxAcceleration.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Jerk</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.jerk.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.jerk.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">P. Jerk Displacement</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.positionProfile.positiveJerkDisplacement.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.positionProfile.positiveJerkDisplacement.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">N. Jerk Displacement</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.positionProfile.negativeJerkDisplacement.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.positionProfile.negativeJerkDisplacement.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">C. Acc Displacement</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.positionProfile.constantAccelerationDisplacement.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.positionProfile.constantAccelerationDisplacement.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">C. Vel Displacement</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.positionProfile.constantVelocityDisplacement.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.positionProfile.constantVelocityDisplacement.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Jerk Duration</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.timeProfile.jerkDuration.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.timeProfile.jerkDuration.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Const. Acceleration</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.timeProfile.constantAccelerationDuration.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.timeProfile.constantAccelerationDuration.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Const. Velocity</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.timeProfile.constantVelocityDuration.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.timeProfile.constantVelocityDuration.getUnitInfo().toString()}
                            </div>
                        </div>
                        <div className="flex">
                            <div className="w-[160px] text-sm text-nowrap">Total Duration</div>
                            <div className="px-2 flex items-center text-pink-400">
                                {data.profile.timeProfile.totalDuration.value.toFixed(2)}
                            </div>
                            <div className="flex items-center text-sm">
                                {data.profile.timeProfile.totalDuration.getUnitInfo().toString()}
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