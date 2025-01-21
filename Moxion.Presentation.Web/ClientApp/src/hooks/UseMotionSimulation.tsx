import MotionSimulationData from "../models/kinematics/MotionSimulationData.ts";
import MotionSimulationParam from "../models/kinematics/MotionSimulationParam.ts";
import ErrorCode from "../models/enumerations/ErrorCode.ts";
import {createContext, ReactNode, useCallback, useContext, useState} from "react";
import Position from "../models/values/Position.ts";
import Velocity from "../models/values/Velocity.ts";
import Acceleration from "../models/values/Acceleration.ts";
import Jerk from "../models/values/Jerk.ts";
import KinematicApi from "../api/KinematicApi.ts";
import KinematicSimulateRequest from "../api/requests/KinematicSimulateRequest.ts";
import Point from "../models/graphics/Point.ts";
import MotionPhaseData from "../models/kinematics/MotionPhaseData.ts";
import MotionProfile from "../models/kinematics/MotionProfile.ts";
import KinematicUnitInfo from "../models/kinematics/units/KinematicUnitInfo.ts";

interface IMotionSimulation {
    param: MotionSimulationParam,
    data: MotionSimulationData,
    isLoading: boolean,
    error: ErrorCode,

    setParam(param: MotionSimulationParam): void,

    simulate(): void
}

function dummyFunction() {
    // Do Nothing
}

const defaultMotionSimulationParam = new MotionSimulationParam(
    new Position(100, 'Millimeter'),
    new Velocity(10, 'Millimeter', 'Second'),
    new Acceleration(5, 'Millimeter', 'Second'),
    new Jerk(1, 'Millimeter', 'Second'),
    'Second',
    100
)

const MotionSimulationContext = createContext<IMotionSimulation>({
    param: defaultMotionSimulationParam,
    data: new MotionSimulationData(),
    isLoading: false,
    error: 'NoError',
    setParam: dummyFunction,
    simulate: dummyFunction
})

interface IMotionSimulationProviderProps {
    children?: ReactNode
}

export function MotionSimulationProvider({children}: IMotionSimulationProviderProps) {
    const [param, setParam] = useState<MotionSimulationParam>(defaultMotionSimulationParam)
    const [data, setData] = useState<MotionSimulationData>(new MotionSimulationData())
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<ErrorCode>('NoError')

    const simulate = useCallback(async () => {
        setIsLoading(true)
        const response = await KinematicApi.simulate(KinematicSimulateRequest.fromParam(param))

        if (!response || !response.data) {
            setError(response?.errorCode ?? 'UnknownError')
            setIsLoading(false)
            return
        }

        const positionData: Point[] = []
        const velocityData: Point[] = []
        const accelerationData: Point[] = []
        const jerkData: Point[] = []
        const phaseData: MotionPhaseData[] = []

        for (let i = 0; i < response.data.length; i++) {
            const time = response.data[i].time;

            positionData.push({x: time, y: response.data[i].position});
            velocityData.push({x: time, y: response.data[i].velocity});
            accelerationData.push({x: time, y: response.data[i].acceleration});
            jerkData.push({x: time, y: response.data[i].jerk});
            phaseData.push({time: time, phase: response.data[i].phase});
        }

        const simulationData = new MotionSimulationData(
            response.profile ?? new MotionProfile(),
            positionData,
            velocityData,
            accelerationData,
            jerkData,
            phaseData,
            response.unitInfo ?? new KinematicUnitInfo()
        );

        setData(simulationData)
        setError(response.errorCode)
        setIsLoading(false)
    }, [param])

    return (
        <MotionSimulationContext.Provider value={{
            param,
            data,
            isLoading,
            error,
            setParam,
            simulate
        }}>
            {children}
        </MotionSimulationContext.Provider>
    )
}

export default function useMotionSimulation() {
    return useContext(MotionSimulationContext)
}