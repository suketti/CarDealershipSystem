import api from "./axiosInstance"; // Import the Axios instance

// Get all body types
export const getAllBodyTypes = async () => {
    const response = await api.get("/cars/metadata/bodytypes");
    return response.data;
};

// Get body type by ID
export const getBodyTypeById = async (id: number) => {
    const response = await api.get(`/cars/metadata/bodytypes/${id}`);
    return response.data;
};

// Get all transmission types
export const getAllTransmissionTypes = async () => {
    const response = await api.get("/cars/metadata/tranmissionTypes");
    return response.data;
};

// Get transmission type by ID
export const getTransmissionTypeById = async (id: number) => {
    const response = await api.get(`/cars/metadata/transmissionTypes/${id}`);
    return response.data;
};

// Get all fuel types
export const getAllFuelTypes = async () => {
    const response = await api.get("/cars/metadata/fuelTypes");
    return response.data;
};

// Get fuel type by ID
export const getFuelTypeById = async (id: number) => {
    const response = await api.get(`/cars/metadata/fuelTypes/${id}`);
    return response.data;
};

// Get all drivetrain types
export const getAllDrivetrainTypes = async () => {
    const response = await api.get("/cars/metadata/drivetrainTypes");
    return response.data;
};

// Get drivetrain type by ID
export const getDrivetrainTypeById = async (id: number) => {
    const response = await api.get(`/cars/metadata/drivetrainTypes/${id}`);
    return response.data;
};

// Get all colors
export const getAllColors = async () => {
    const response = await api.get("/cars/metadata/colors");
    return response.data;
};

// Get color by ID
export const getColorById = async (id: number) => {
    const response = await api.get(`/cars/metadata/colors/${id}`);
    return response.data;
};


//Get all Makers
export const getAllMakerTypes = async () => {
    const response = await api.get("/cars/makers");
    return response.data;
};

//Get all Models
export const getAllModelsTypes = async () => {
    const response = await api.get("/cars/models");
    return response.data;
};