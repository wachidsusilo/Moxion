import PositionUnit from "../models/enumerations/PositionUnit.ts";
import TimeUnit from "../models/enumerations/TimeUnit.ts";

class UnitFormatter {
    static toPositionDisplayFormat(unit: PositionUnit) {
        switch (unit) {
            case "Kilometer":
                return 'km'
            case "Hectometer":
                return 'ha'
            case "Decameter":
                return "dam";
            case 'Meter':
                return 'm'
            case "Decimeter":
                return 'dm'
            case "Centimeter":
                return 'cm'
            case "Millimeter":
                return 'mm'
            default:
                return ''
        }
    }

    static toTimeDisplayFormat(unit: TimeUnit) {
        switch (unit) {
            case "Second":
                return 's'
            case "Millisecond":
                return 'ms'
            case "Microsecond":
                return 'us'
            case "Nanosecond":
                return 'ns'
            default:
                return ''
        }
    }
}

export default UnitFormatter