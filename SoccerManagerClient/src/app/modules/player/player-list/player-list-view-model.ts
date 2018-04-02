import { Player } from './../player';
export class PlayerListViewModel extends Player {
    get squadsAsString(){
        return "Kampfmannschaft, Reservemannschaft";
    }
}
