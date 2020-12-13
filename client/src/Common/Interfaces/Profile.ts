import Availability from "../Enums/Availability";
import CyclingType from "../Enums/CyclingType";

export default interface Profile {
    userId: string,
    displayName: string,
    locationName: string,
    cyclingTypes: Array<CyclingType>,
    availability: Array<Availability>,
    minDistance: number,
    maxDistance: number,
    speed: number,
    distanceFromUserKM: number,
    profileImage?: string
}