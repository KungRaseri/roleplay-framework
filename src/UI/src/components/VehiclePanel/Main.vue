<template>
    <v-container class="outter">
        <v-slide-y-reverse-transition>
            <v-container class="panel">
                <v-card v-show="IsVehiclePanelActive" class="card" color="rgba(0, 0, 0, 0.9)" rounded="lg">
                    <v-container class="inset">
                        <v-row class="outer-row" dense>
                            <v-col cols="4">
                                <ToggleButton type="engine" :seat="Seat" icon="mdi-power" />
                            </v-col>
                            <v-col cols="2">
                                <ToggleButton type="door" :index="4" :seat="Seat" icon="mdi-car" />
                            </v-col>
                            <v-col cols="2">
                                <ToggleButton type="door" :index="5" :seat="Seat" icon="mdi-car-back" />
                            </v-col>
                            <v-col cols="4">
                                <ToggleButton type="belt" icon="mdi-seatbelt" :isCar="IsCar" />
                            </v-col>
                        </v-row>
                        <v-row class="outer-row" dense>
                            <v-col>
                                <ToggleButton type="door" :index="0" :seat="Seat" icon="mdi-car-door" />
                            </v-col>
                            <v-col>
                                <ToggleButton type="window" :index="0" :seat="Seat" icon="mdi-window-closed" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <SeatButton :index="-1" :seat="Seat" :taken="Taken[0]" icon="mdi-seat-passenger mdi-flip-h" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <SeatButton :index="0" :seat="Seat" :taken="Taken[1]" icon="mdi-seat-passenger" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <ToggleButton type="window" :index="1" :seat="Seat" icon="mdi-window-closed" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <ToggleButton type="door" :index="1" :seat="Seat" icon="mdi-car-door mdi-flip-h" />
                            </v-col>
                        </v-row>
                        <v-row class="outer-row" dense>
                            <v-col>
                                <ToggleButton type="door" :index="2" :seat="Seat" icon="mdi-car-door mdi-flip-v" />
                            </v-col>
                            <v-col>
                                <ToggleButton type="window" :index="2" :seat="Seat" icon="mdi-window-closed" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <SeatButton :index="1" :seat="Seat" :taken="Taken[2]" icon="mdi-seat-passenger mdi-rotate-180" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <SeatButton :index="2" :seat="Seat" :taken="Taken[3]" icon="mdi-seat-passenger mdi-flip-v" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <ToggleButton type="window" :index="3" :seat="Seat" icon="mdi-window-closed" :seats="Seats" />
                            </v-col>
                            <v-col>
                                <ToggleButton type="door" :index="3" :seat="Seat" icon="mdi-car-door mdi-rotate-180" />
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card>
            </v-container>
        </v-slide-y-reverse-transition>
    </v-container>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import ToggleButton from './ToggleButton.vue';
    import SeatButton from './SeatButton.vue';

    @Component({
        components: {
            ToggleButton,
            SeatButton
        },
    })
    export default class VehiclePanel extends Vue {
        isVehiclePanelActive = false;
        seat = -1;
        taken = [true, false, false, false];
        seats = 1;
        isCar = false;
        $axios: any;

        mounted() {
            window.addEventListener("message", (e) => {
                switch (e.data.eventName) {
                    case "TOGGLE_VEHICLE_PANEL_MODULE":
                        this.IsVehiclePanelActive = e.data.visible;
                        break;
                    case "VEHICLE_PANEL_DATA":
                        this.PanelStatus(e.data);
                        break;
                    default:
                        break;
                }
            });

            window.addEventListener("keydown", (e) => {
                if (e.key === "Escape") {
                    this.IsVehiclePanelActive = false;
                    this.ResetFocus();
                }
            });
        }

        ResetFocus() {
            this.$axios
                .post(
                    "http://framework/RESET_FOCUS",
                    {}
                )
                .catch((error: any) => {
                    console.log("error", error);
                });
        }

        PanelStatus(value: any) {
            this.Seat = value.Seat;
            this.Taken = value.Taken;
            this.Seats = value.Seats;
            this.IsCar = value.isCar
        }

        GetStatus(value: any) {
            if (value) {
                return "green";
            }
            return "";
        }

        get IsVehiclePanelActive() {
            return this.isVehiclePanelActive;
        }

        set IsVehiclePanelActive(value: boolean) {
            this.isVehiclePanelActive = value;
        }

        get Seat() {
            return this.seat;
        }

        set Seat(value: number) {
            this.seat = value;
        }

        get Taken() {
            return this.taken;
        }

        set Taken(value: boolean[]) {
            this.taken = value;
        }

        get Seats() {
            return this.seats;
        }

        set Seats(value: number) {
            this.seats = value;
        }

        get IsCar() {
            return this.isCar;
        }

        set IsCar(value: boolean) {
            this.isCar = value;
        }
    }
</script>

<style>
    .button-seat-in {
        pointer-events: none;
        background-color: rgb(0 128 0 / 0.53) !important;
    }

    .button-unavailable {
        pointer-events: none;
        background-color: rgb(128 0 0 / 0.53) !important;
    }
</style>

<style scoped>
    .panel {
        height: 100%;
        width: 25%;
        margin-left: auto;
        margin-right: auto;
        padding: 0;
    }

    .card {
        position: absolute;
        width: 25%;
        bottom: 5%;
        z-index: 100;
    }

    .inset {
        height: 100%;
        padding: 10px !important;
    }

    .top-row {
        height: 20%;
        flex-wrap: nowrap;
    }

    .outer-row {
        height: 5vh;
        flex-wrap: nowrap;
    }

    .v-btn {
        padding: 0 !important;
    }

    .button-seat-in {
        pointer-events: none;
        background-color: rgb(0 128 0 / 0.53) !important;
    }

    .button-seat {
        pointer-events: none;
        background-color: rgb(128 0 0 / 0.53) !important;
    }

    .v-card {
        padding: 0 !important;
    }
</style>