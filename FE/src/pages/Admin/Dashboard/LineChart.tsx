// import React from "react";
import { ResponsiveLine } from "@nivo/line";
import { showDailyRevenueSummary } from "@/services/orderAPI";
import { useEffect, useState } from "react";

const LineChart = ({ isDashboard = false }) => {
    const [dailyRevenues, setDailyRevenues] = useState<any[]>([]);

    const fetchData = async () => {
        try {
            const response = await showDailyRevenueSummary();
            // Assuming the API response structure is { data: { data: { DailyRevenueResults: [...] } } }
            const dailyData = Array.isArray(response.data) ? response.data : [];

            const formattedData = dailyData.map((item: any) => {
                const date = new Date(item.date);
                const month = (date.getMonth() + 1).toString().padStart(2, '0');
                const day = date.getDate().toString().padStart(2, '0');
                return {
                    x: `${month}-${day}`,
                    y: item.totalRevenue,
                };
            });
            setDailyRevenues(formattedData);
        } catch (error) {
            console.error("Failed to fetch daily revenue info:", error);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const lineColors = { green: "#15F5BA", pink: "#FF9EAA", purple: "#912BBC" };

    const data = [
        {
            id: "Daily Revenue",
            color: lineColors.purple,
            data: dailyRevenues,
        },
    ];

    const colors = { primary: "#151542" };

    return (
        <ResponsiveLine
            data={data}
            theme={{
                axis: {
                    domain: {
                        line: {
                            stroke: colors.primary,
                        },
                    },
                    legend: {
                        text: {
                            fill: colors.primary,
                        },
                    },
                    ticks: {
                        line: {
                            stroke: colors.primary,
                            strokeWidth: 1,
                        },
                        text: {
                            fill: colors.primary,
                        },
                    },
                },
                legends: {
                    text: {
                        fill: colors.primary,
                    },
                },
                tooltip: {
                    container: {
                        color: colors.primary,
                    },
                },
            }}
            colors={{ datum: "color" }}
            margin={{ top: 50, right: 110, bottom: 50, left: 60 }}
            xScale={{ type: "point" }}
            yScale={{
                type: "linear",
                min: "auto",
                max: "auto",
                stacked: true,
                reverse: false,
            }}
            yFormat=" >-.2f"
            curve="catmullRom"
            axisTop={null}
            axisRight={null}
            axisBottom={{
                tickSize: 0,
                tickPadding: 5,
                tickRotation: 0,
                legend: isDashboard ? undefined : "Days", // Changed from Months to Days
                legendOffset: 36,
                legendPosition: "middle",
            }}
            axisLeft={{
                tickValues: 5,
                tickSize: 3,
                tickPadding: 5,
                tickRotation: 0,
                legend: isDashboard ? undefined : "Value",
                legendOffset: -40,
                legendPosition: "middle",
            }}
            enableGridX={false}
            enableGridY={false}
            pointSize={8}
            pointColor={{ theme: "background" }}
            pointBorderWidth={2}
            pointBorderColor={{ from: "serieColor" }}
            pointLabelYOffset={-12}
            useMesh={true}
            legends={[
                {
                    anchor: "bottom-right",
                    direction: "column",
                    justify: false,
                    translateX: 100,
                    translateY: 0,
                    itemsSpacing: 0,
                    itemDirection: "left-to-right",
                    itemWidth: 80,
                    itemHeight: 20,
                    itemOpacity: 0.75,
                    symbolSize: 12,
                    symbolShape: "circle",
                    symbolBorderColor: "rgba(0, 0, 0, .5)",
                    effects: [
                        {
                            on: "hover",
                            style: {
                                itemBackground: "rgba(0, 0, 0, .03)",
                                itemOpacity: 1,
                            },
                        },
                    ],
                },
            ]}
        />
    );
};

export default LineChart;
