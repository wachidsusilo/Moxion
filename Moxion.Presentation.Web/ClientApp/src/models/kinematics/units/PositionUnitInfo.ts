import PositionUnit from "../../enumerations/PositionUnit.ts";
import UnitFormatter from "../../../utilities/UnitFormatter.ts";

class PositionUnitInfo {
    unit: PositionUnit

    constructor(unit: PositionUnit) {
        this.unit = unit
    }

    toString() {
        return UnitFormatter.toPositionDisplayFormat(this.unit)
    }
}

export default PositionUnitInfo