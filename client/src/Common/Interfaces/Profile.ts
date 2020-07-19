import CyclingType from "../Enums/CyclingType";

export interface Profile {
    name: string
    placeName: string
    preferredCyclingTypes: [CyclingType]
    createdAt: Date
}