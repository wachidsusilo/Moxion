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

    static from(other: PositionUnitInfo) {
        if (!other) {
            return new PositionUnitInfo('None')
        }

        return new PositionUnitInfo(other.unit)
    }
}

export default PositionUnitInfo