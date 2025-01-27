import {
    VictoryAxis,
    VictoryAxisProps,
    VictoryChart, VictoryChartProps, VictoryLegend, VictoryLegendProps,
    VictoryLine,
    VictoryLineProps,
    VictoryScatter,
    VictoryScatterProps, VictoryStyleObject,
    VictoryTooltip,
    VictoryVoronoiContainer
} from "victory";
import MotionSimulationData from "../../models/kinematics/MotionSimulationData.ts";
import {Dispatch, SetStateAction, useState} from "react";
import Point from "../../models/graphics/Point.ts";
import MotionPhaseData from "../../models/kinematics/MotionPhaseData.ts";
import {FaEye, FaEyeSlash} from "react-icons/fa6";

interface MotionChartProps {
    data: MotionSimulationData
}

interface SingleChartProps {
    title: string,
    pointData: Point[],
    phaseData: MotionPhaseData[],
    color: string
}

interface SingleChartCollectionProps {
    data: MotionSimulationData
}

interface MultiChartProps {
    data: MotionSimulationData
}

interface EyeLegendIconProps {
    index?: number
    style?: VictoryStyleObject,
    x?: number
    y?: number
    chartVisibleList: boolean[]
    setPositionVisible: Dispatch<SetStateAction<boolean>>
    setVelocityVisible: Dispatch<SetStateAction<boolean>>
    setAccelerationVisible: Dispatch<SetStateAction<boolean>>
    setJerkVisible: Dispatch<SetStateAction<boolean>>
}

const singleChartProps: VictoryChartProps = {
    width: 450,
    height: 250,
    padding: {
        top: 30,
        right: 30,
        bottom: 50,
        left: 50
    },
    containerComponent: <VictoryVoronoiContainer/>
}

const multiChartProps: VictoryChartProps = {
    width: 900,
    height: 540,
    padding: {
        top: 50,
        right: 50,
        bottom: 70,
        left: 50
    },
    containerComponent: <VictoryVoronoiContainer voronoiPadding={{top: 50, right: 50, bottom: 50, left: 50}}/>
}

const crossAxisProps: VictoryAxisProps = {
    crossAxis: true,
    style: {
        axis: {stroke: 'gray'},
        tickLabels: {fill: '#aaaaaa', fontSize: 10, fontFamily: 'Montserrat Alternates'}
    }
}

const dependentAxisProps: VictoryAxisProps = {
    dependentAxis: true,
    style: {
        axis: {stroke: 'gray'},
        tickLabels: {fill: '#aaaaaa', fontSize: 10, fontFamily: 'Montserrat Alternates'}
    }
}

function scatterProps(
    seriesName: string | null,
    data: Point[],
    tooltipData: Point[],
    phaseData: MotionPhaseData[],
    fill: string,
    opacity: number,
    size: number
): VictoryScatterProps {
    return {
        data,
        size,
        style: {
            data: {fill, opacity}
        },
        labels: (e) => `${seriesName ? `${seriesName}\n` : ''}${phaseData[e.index].phase}\n(${tooltipData[e.index].x.toFixed(2)}, ${tooltipData[e.index].y.toFixed(2)})`,
        labelComponent: <VictoryTooltip style={{fontSize: 10, fill: "#eaeaea", fontFamily: 'Montserrat Alternates'}}
                                        flyoutStyle={{fill: "black", stroke: "gray"}}/>
    }
}

function lineProps(
    data: Point[],
    stroke: string,
    opacity: number,
    strokeWidth: number
): VictoryLineProps {
    return {
        data,
        style: {
            data: {stroke, strokeWidth, opacity}
        }
    }
}

function multiChartLegendProps(
    chartVisibleList: boolean[],
    setPositionVisible: Dispatch<SetStateAction<boolean>>,
    setVelocityVisible: Dispatch<SetStateAction<boolean>>,
    setAccelerationVisible: Dispatch<SetStateAction<boolean>>,
    setJerkVisible: Dispatch<SetStateAction<boolean>>
): VictoryLegendProps {
    function onClick(_: any, {index}: any) {
        switch (index) {
            case 0:
                setPositionVisible(state => !state)
                break
            case 1:
                setVelocityVisible(state => !state)
                break
            case 2:
                setAccelerationVisible(state => !state)
                break
            case 3:
                setJerkVisible(state => !state)
                break
        }
    }

    return {
        itemsPerRow: 1,
        x: 50,
        y: 510,
        gutter: 50,
        data: [
            {name: 'Position', symbol: {fill: 'blue', type: 'minus'}},
            {name: 'Velocity', symbol: {fill: 'green', type: 'minus'}},
            {name: 'Acceleration', symbol: {fill: 'purple', type: 'minus'}},
            {name: 'Jerk', symbol: {fill: 'orange', type: 'minus'}}
        ],
        dataComponent: <EyeLegendIcon chartVisibleList={chartVisibleList} 
                                      setPositionVisible={setPositionVisible}
                                      setVelocityVisible={setVelocityVisible}
                                      setAccelerationVisible={setAccelerationVisible}
                                      setJerkVisible={setJerkVisible}/>,
        style: {
            data: {cursor: 'pointer'},
            labels: {fill: '#cacaca', fontSize: 10, fontFamily: 'Montserrat Alternates', cursor: 'pointer'}
        },
        events: [
            {target: 'data', eventHandlers: {onClick}},
            {target: 'labels', eventHandlers: {onClick}}
        ]
    }
}

function EyeLegendIcon(
    {
        index,
        style,
        x,
        y,
        chartVisibleList,
        setPositionVisible,
        setVelocityVisible,
        setAccelerationVisible,
        setJerkVisible
    }: EyeLegendIconProps
) {
    function onClick(_: any) {
        switch (index) {
            case 0:
                setPositionVisible(state => !state)
                break
            case 1:
                setVelocityVisible(state => !state)
                break
            case 2:
                setAccelerationVisible(state => !state)
                break
            case 3:
                setJerkVisible(state => !state)
                break
        }
    }

    return chartVisibleList[index!]
        ? <FaEye x={x! - 7} y={y! - 7} size={15} fill={style?.fill as string} cursor='pointer' onClick={onClick}/>
        : <FaEyeSlash x={x! - 7} y={y! - 7} size={15} fill='#aaaaaa' cursor='pointer' onClick={onClick}/>
}

function SingleChart({title, pointData, phaseData, color}: SingleChartProps) {
    return (
        <div className="flex flex-col items-center">
            <div>{title}</div>
            <VictoryChart {...singleChartProps}>
                <VictoryAxis {...crossAxisProps} />
                <VictoryAxis {...dependentAxisProps} />
                <VictoryScatter {...scatterProps(null, pointData, pointData, phaseData, color, 1, 1.5)}/>
                <VictoryLine {...lineProps(pointData, color, 1, 1)}/>
            </VictoryChart>
        </div>
    )
}

function SingleChartCollection({data}: SingleChartCollectionProps) {
    return (
        <div className="grid grid-cols-2 grid-rows-2 gap-8">
            <SingleChart title='Position'
                         pointData={data.positionData}
                         phaseData={data.phaseData}
                         color='blue'/>
            <SingleChart title='Velocity'
                         pointData={data.velocityData}
                         phaseData={data.phaseData}
                         color='green'/>
            <SingleChart title='Acceleration'
                         pointData={data.accelerationData}
                         phaseData={data.phaseData}
                         color='purple'/>
            <SingleChart title='Jerk'
                         pointData={data.jerkData}
                         phaseData={data.phaseData}
                         color='orange'/>
        </div>
    )
}

function normalize(value: Point, maxValue: number, maxPosition: number, factor: number): Point {
    return {
        x: value.x,
        y: maxValue == 0
            ? 0
            : value.y * maxPosition * factor / maxValue
    }
}

function MultiChart({data}: MultiChartProps) {
    const [positionVisible, setPositionVisible] = useState<boolean>(true);
    const [velocityVisible, setVelocityVisible] = useState<boolean>(true);
    const [accelerationVisible, setAccelerationVisible] = useState<boolean>(true);
    const [jerkVisible, setJerkVisible] = useState<boolean>(true);

    const maxPosition = data.positionData[data.positionData.length - 1]?.y;
    const maxVelocity = data.profile.maxVelocity.value;
    const maxAcceleration = data.profile.maxAcceleration.value;
    const maxJerk = data.profile.jerk.value;

    const normalizedVelocityData: Point[] = data.velocityData.map(point => normalize(point, maxVelocity, maxPosition, 0.8))
    const normalizedAccelerationData: Point[] = data.accelerationData.map(point => normalize(point, maxAcceleration, maxPosition, 0.4))
    const normalizedJerkData: Point[] = data.jerkData.map(point => normalize(point, maxJerk, maxPosition, 0.4))

    const chartVisibleList = [positionVisible, velocityVisible, accelerationVisible, jerkVisible]

    return (
        <VictoryChart {...multiChartProps}>
            <VictoryAxis {...crossAxisProps} />
            <VictoryAxis {...dependentAxisProps} />
            {
                jerkVisible &&
                <VictoryScatter {...scatterProps("Jerk", normalizedJerkData, data.jerkData, data.phaseData, 'orange', 1, 2)}/>
            }
            {
                jerkVisible &&
                <VictoryLine {...lineProps(normalizedJerkData, 'orange', 1, 1)}/>
            }
            {
                accelerationVisible &&
                <VictoryScatter {...scatterProps("Acceleration", normalizedAccelerationData, data.accelerationData, data.phaseData, 'purple', 1, 2)}/>
            }
            {
                accelerationVisible &&
                <VictoryLine {...lineProps(normalizedAccelerationData, 'purple', 1, 1)}/>
            }
            {
                velocityVisible &&
                <VictoryScatter {...scatterProps("Velocity", normalizedVelocityData, data.velocityData, data.phaseData, 'green', 1, 2)}/>
            }
            {
                velocityVisible &&
                <VictoryLine {...lineProps(normalizedVelocityData, 'green', 1, 1)}/>
            }
            {
                positionVisible &&
                <VictoryScatter {...scatterProps("Position", data.positionData, data.positionData, data.phaseData, 'blue', 1, 2.5)}/>
            }
            {
                positionVisible &&
                <VictoryLine {...lineProps(data.positionData, 'blue', 1, 1)}/>
            }
            <VictoryLegend {...multiChartLegendProps(chartVisibleList, setPositionVisible, setVelocityVisible, setAccelerationVisible, setJerkVisible)}/>
        </VictoryChart>
    )
}

function MotionChart({data}: MotionChartProps) {
    const [combined, setCombined] = useState<boolean>(true);

    return (
        <div className="relative w-full h-[calc(100vh-92px)]">
            <div className="w-full h-full p-8 overflow-auto">
                <div>
                    {
                        combined
                            ? <MultiChart data={data}/>
                            : <SingleChartCollection data={data}/>
                    }
                </div>
            </div>
            <button onClick={() => setCombined(!combined)}
                    className="absolute top-6 right-8 w-[100px] h-[30px] button rounded-[4px] text-sm bg-blue-500/15 hover:bg-blue-500/30 border border-blue-500/50">
                {combined ? 'Separate' : 'Combine'}
            </button>
        </div>
    )
}

export default MotionChart