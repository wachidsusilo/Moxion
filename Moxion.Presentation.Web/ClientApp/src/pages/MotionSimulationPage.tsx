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
                <div className="w-[500px] px-8 flex flex-col gap-4 shadow-[1px_0px_0px_0px_rgba(255_255_255_/_0.1)]">
                    <div className="mt-8 flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Displacement</div>
                        <input ref={displacementRef}
                               className="h-[30px] w-[100px] p-2 outline outline-1 outline-green-500" type="number"/>
                        <div>{param.displacement.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Velocity</div>
                        <input ref={velocityRef} className="h-[30px] w-[100px] p-2 outline outline-1 outline-green-500"
                               type="number"/>
                        <div>{param.velocity.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Acceleration</div>
                        <input ref={accelerationRef}
                               className="h-[30px] w-[100px] p-2 outline outline-1 outline-green-500" type="number"/>
                        <div>{param.acceleration.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Jerk</div>
                        <input ref={jerkRef} className="h-[30px] w-[100px] p-2 outline outline-1 outline-green-500"
                               type="number"/>
                        <div>{param.jerk.getUnitInfo().toString()}</div>
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-[130px]  shrink-0">Data Count</div>
                        <input ref={dataCountRef} className="h-[30px] w-[100px] p-2 outline outline-1 outline-green-500"
                               type="number"/>
                    </div>
                    <div className="w-full mt-8 flex items-center justify-center">
                        <button className="w-[200px] h-[35px] flex items-center justify-center bg-blue-500/50"
                                disabled={isLoading}
                                onClick={onClick}>
                            Simulate
                        </button>
                    </div>
                    <div className="w-full flex items-center justify-center">
                        <div className="flex gap-3">
                            <div>Error:</div>
                            <div>{error}</div>
                        </div>
                    </div>
                </div>
                <MotionChart data={data}/>
            </div>
        </div>
    )
}

export default MotionSimulationPage