import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PartnerService } from "../services/partner.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { PlayerService } from '../services/player.service';
import { Player } from '../models/player.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-add-partner',
  templateUrl: './add-partner.component.html'
})
export class AddPartnerComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private router: Router, private partnerService: PartnerService, private _playerService: PlayerService, private _userService: UserService) {
    this.getPlayers();
  }

  allPlayers: Player[]
  createPartnerForm: FormGroup;

  ngOnInit() {

    this.createPartnerForm = this.formBuilder.group({
      id: [],
      partnerId: ['', Validators.required]
    });

  }

  getPlayers() {
    this._userService.getPlayerid().subscribe(playerId => {
      this._playerService.getPlayers().subscribe(data => {
        this.allPlayers = data;
        var index = this.allPlayers.map(player => player.id).indexOf(playerId);
        this.allPlayers.splice(index, 1);
      });
    });
  }

  onSubmit() {
    this.partnerService.addPartner(this.createPartnerForm.value)
      .subscribe(data => {
        this.router.navigate(['partners']);
      });
  }

}
