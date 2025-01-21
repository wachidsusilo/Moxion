import PositionUnit from "../../enumerations/PositionUnit.ts";
import TimeUnit from "../../enumerations/TimeUnit.ts";
import UnitFormatter from "../../../utilities/UnitFormatter.ts";

class AccelerationUnitInfo {
    positionUnit: PositionUnit
    timeUnit: TimeUnit

    constructor(positionUnit: PositionUnit, timeUnit: TimeUnit) {
        this.positionUnit = positionUnit
        this.timeUnit = timeUnit
    }

    toString() {
        return `${UnitFormatter.toPositionDisplayFormat(this.positionUnit)}/${UnitFormatter.toTimeDisplayFormat(this.timeUnit)}²`
    }
}

export default AccelerationUnitInfo