import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registrar',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    MessageModule,
    ToastModule
  ],
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.css']
})
export class RegistrarComponent {
  form: FormGroup;

  constructor(private fb: FormBuilder,private authService: AuthService,private messageService: MessageService,
    private router:Router
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      nomeCompleto: ['', Validators.required],
      senha: ['', [Validators.required, Validators.minLength(6)]],
      confirmarSenha: ['', Validators.required]
    }, { validators: this.confirmarSenhasIguais });
  }

  confirmarSenhasIguais(group: AbstractControl): ValidationErrors | null {
    const senha = group.get('senha')?.value;
    const confirmarSenha = group.get('confirmarSenha')?.value;
    return senha === confirmarSenha ? null : { senhasDiferentes: true };
  }

  campoInvalido(campo: string): boolean {
    const control = this.form.get(campo);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  get senhasDiferentes(): boolean {
    return this.form.errors?.['senhasDiferentes'] && (this.form.get('confirmarSenha')?.touched || this.form.get('confirmarSenha')?.dirty);
  }

  registrar(): void {
    if (this.form.invalid) return;

    const { nomeCompleto, email, senha } = this.form.value;

    this.authService.registrar({
      nomeCompleto,
      email,
      senhaDigitada: senha
    }).subscribe({
      next: (res) => {

        this.messageService.add({
          severity: 'success',
          summary: 'Registro realizado',
          detail: 'Usuário registrado com sucesso!',
          life: 4000
        });
        localStorage.removeItem('token');
        sessionStorage.removeItem('token');
        sessionStorage.setItem('token', res.token);
        this.router.navigate(['/dashboard/pedidos']);
      },
      error: (err) => {
        const erro = err?.error?.erro || 'Erro desconhecido ao registrar.';

        this.messageService.add({
          severity: 'error',
          summary: 'Erro ao registrar',
          detail: erro,
          life: 5000
        });
      }
    });
  }
}
