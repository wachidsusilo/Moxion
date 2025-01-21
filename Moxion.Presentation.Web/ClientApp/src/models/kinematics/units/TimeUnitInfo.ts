import TimeUnit from "../../enumerations/TimeUnit.ts";
import UnitFormatter from "../../../utilities/UnitFormatter.ts";

class TimeUnitInfo {
    unit: TimeUnit

    constructor(unit: TimeUnit) {
        this.unit = unit
    }

    toString() {
        return UnitFormatter.toTimeDisplayFormat(this.unit)
    }
}

export default TimeUnitInfo