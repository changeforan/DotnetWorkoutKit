import Foundation

class Helper {
    static func convertPace2Speed(pace: String) -> Double {
        let paceWithoutQuote = pace.replacingOccurrences(of: "\"", with: "")
        let components = paceWithoutQuote.components(separatedBy: "'")
        guard components.count == 2 else {
            return 0.0
        }
        let minutes = Int(components[0]) ?? 0
        let seconds = Int(components[1]) ?? 0
        let totalSecondsPerKm = minutes * 60 + seconds
        let speedKph = 3600 / Double(totalSecondsPerKm)
        return speedKph
    }
}
