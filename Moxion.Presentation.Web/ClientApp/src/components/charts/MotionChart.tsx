import {VictoryAxis, VictoryChart, VictoryLine, VictoryScatter} from "victory";
import MotionSimulationData from "../../models/kinematics/MotionSimulationData.ts";

interface MotionChartProps {
    data: MotionSimulationData
}

function MotionChart({data}: MotionChartProps) {
    return (
        <div className="p-8 w-full grid grid-cols-2 grid-rows-2 gap-8">
            <div className="flex flex-col items-center">
                <div>Position</div>
                <VictoryChart height={250} colorScale='blue' padding={{left: 50, top: 50, bottom: 50, right: 50}}>
                    <VictoryAxis crossAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}} />
                    <VictoryAxis dependentAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}}/>
                    <VictoryScatter data={data.positionData} style={{data: {stroke: 'blue', strokeWidth: 1}}} size={1}/>
                    <VictoryLine data={data.positionData} style={{data: {stroke: 'blue', strokeWidth: 1}}}/>
                </VictoryChart>
            </div>
            <div className="flex flex-col items-center">
                <div>Velocity</div>
                <VictoryChart height={250} colorScale='blue' padding={{left: 50, top: 50, bottom: 50, right: 50}}>
                    <VictoryAxis crossAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}} />
                    <VictoryAxis dependentAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}}/>
                    <VictoryScatter data={data.velocityData} style={{data: {stroke: 'green', strokeWidth: 1}}} size={1}/>
                    <VictoryLine data={data.velocityData} style={{data: {stroke: 'green', strokeWidth: 1}}}/>
                </VictoryChart>
            </div>
            <div className="flex flex-col items-center">
                <div>Acceleration</div>
                <VictoryChart height={250} colorScale='blue' padding={{left: 50, top: 50, bottom: 50, right: 50}}>
                    <VictoryAxis crossAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}} />
                    <VictoryAxis dependentAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}}/>
                    <VictoryScatter data={data.accelerationData} style={{data: {stroke: 'purple', strokeWidth: 1}}} size={1}/>
                    <VictoryLine data={data.accelerationData} style={{data: {stroke: 'purple', strokeWidth: 1}}}/>
                </VictoryChart>
            </div>
            <div className="flex flex-col items-center">
                <div>Jerk</div>
                <VictoryChart height={250} colorScale='blue' padding={{left: 50, top: 50, bottom: 50, right: 50}}>
                    <VictoryAxis crossAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}} />
                    <VictoryAxis dependentAxis style={{axis: {stroke: 'gray'}, tickLabels: { fill: "gray", fontSize: 10 }}}/>
                    <VictoryScatter data={data.jerkData} style={{data: {stroke: 'orange', strokeWidth: 1}}} size={1}/>
                    <VictoryLine data={data.jerkData} style={{data: {stroke: 'orange', strokeWidth: 1}}}/>
                </VictoryChart>
            </div>
        </div>
    )
}

export default MotionChart