import PositionUnit from "../enumerations/PositionUnit.ts";
import PositionUnitInfo from "../kinematics/units/PositionUnitInfo.ts";

class Position {
    value: number
    unit: PositionUnit

    constructor(value?: number, unit?: PositionUnit) {
        this.value = value ?? 0
        this.unit = unit ?? 'Meter'
    }

    getUnitInfo() {
        return new PositionUnitInfo(this.unit)
    }

    static from(other: Position) {
        if (!other) {
            return new Position()
        }

        return new Position(other.value, other.unit)
    }
}

export default Position