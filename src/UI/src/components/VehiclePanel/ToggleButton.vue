<template>
    <v-btn :class="{'button-unavailable': Broken}"
           height="100%" block dark :color="GetStatus()" @click="ToggleComponent()" :disabled="CheckDisable()">
        <v-icon :size="IconSize()">{{ Icon }}</v-icon>
    </v-btn>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from 'vue-property-decorator';

    @Component({
        components: {
        },
    })
    export default class ToggleButton extends Vue {
        @Prop(String) type = this.Type;
        @Prop(Number) index = this.Index;
        @Prop(Number) seat = this.Seat;
        @Prop(String) icon = this.Icon;
        @Prop(Number) seats = this.Seats;
        @Prop(Boolean) isCar = this.IsCar;

        status = false;
        broken = false;

        $axios: any;

        mounted() {
            window.addEventListener("message", (e) => {
                switch (e.data.eventName) {
                    case "VEHICLE_PANEL_STATE":
                        this.PanelStatus(e.data);
                        break;
                    default:
                        break;
                }
            });
        }

        PanelStatus(value: any) {
            if ((this.Type === value.type && this.Index === value.index) || (this.Type === "engine" && value.type === "engine")) {
                if (this.Status != value.status) {
                    this.Status = value.status;
                }
                if (this.Broken != value.broken) {
                    this.Broken = value.broken;
                }
            }
        }

        IconSize() {
            let size = window.innerWidth + window.innerHeight;
            return size * 0.01;
        }

        GetStatus() {
            if (this.Status) {
                return "green";
            }
            return "";
        }

        ToggleComponent() {
            this.Status = !this.Status;
            let type = this.Type;
            let index = this.Index;
            let status = this.Status;
            this.$axios
                .post(
                    "http://framework/TOGGLE_VEHICLE_COMPONENT",
                    { type, index, status }
                )
                .catch((error: any) => {
                    console.log("error", error);
                });
        }

        CheckDisable() {
            if (this.Type === "engine") {
                if (this.Seat === -1) {
                    return false;
                } else {
                    return true;
                }
            } else if (this.Type === "door" || this.Type === "window") {
                if (this.Seat + 1 === this.Index) {
                    return false;
                } else if (this.Seat === -1 && (this.Index === 4 || this.Index === 5)) {
                    return false;
                } else if (this.Type === "window" && this.Seat === -1) {
                    if (this.Index < this.Seats) {
                        return false;
                    }
                }
            } else if (this.Type === "belt" && this.IsCar) {
                return false;
            }

            return true;
        }

        get Status() {
            return this.status;
        }

        set Status(value: boolean) {
            this.status = value;
        }

        get Broken() {
            return this.broken;
        }

        set Broken(value: boolean) {
            this.broken = value;
        }

        get Type() {
            return this.type;
        }

        set Type(value: string) {
            this.type = value;
        }

        get Index() {
            return this.index;
        }

        set Index(value: number) {
            this.index = value;
        }

        get Seat() {
            return this.seat;
        }

        set Seat(value: number) {
            this.seat = value;
        }

        get Icon() {
            return this.icon;
        }

        set Icon(value: string) {
            this.icon = value;
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

<style scoped>
</style>