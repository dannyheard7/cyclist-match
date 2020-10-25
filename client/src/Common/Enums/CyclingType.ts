enum CyclingType {
    ROAD_BIKE = "ROAD_BIKE",
    MOUNTAIN_BIKE = "MOUNTAIN_BIKE",
    CYCLO_CROSS = "CYCLO_CROSS"
}

export const CyclingTypeName = {
    [CyclingType.ROAD_BIKE]: "Road",
    [CyclingType.MOUNTAIN_BIKE]: "Mountain",
    [CyclingType.CYCLO_CROSS]: "Cylo-Cross"
}

export default CyclingType;