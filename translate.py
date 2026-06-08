import os
import re

directory = "/root/RiderProjects/TradeOffStackAPI/TradeOffStackAPI/Services"

replacements = {
    '"Cet équipement a déjà une demande de maintenance ouverte."': '"This equipment already has an open maintenance request."',
    '"Cet équipement possède déjà une réservation active."': '"This equipment already has an active reservation."',
    '"Demande annulée."': '"Request cancelled."',
    '"Demande créée."': '"Request created."',
    '"Demande de maintenance non trouvée."': '"Maintenance request not found."',
    '"Demande mise à jour."': '"Request updated."',
    '"Demande non trouvée."': '"Request not found."',
    '"Demande terminée."': '"Request completed."',
    '"Échec de la création de la demande."': '"Failed to create the request."',
    '"Échec de la mise à jour."': '"Failed to update."',
    '"Échec de la mise à jour de l\'utilisateur."': '"Failed to update the user."',
    '"Échec de la sauvegarde de la réservation."': '"Failed to save the reservation."',
    '"Échec de la sauvegarde de l\'utilisateur."': '"Failed to save the user."',
    '"Équipement retourné."': '"Equipment returned."',
    '"Erreur de transaction : {ex.Message}"': '"Transaction error: {ex.Message}"',
    '"L\'email est requis."': '"Email is required."',
    '"Le prénom est requis."': '"First name is required."',
    '"L\'équipement n\'est pas disponible. Statut actuel : {equipment.Status}."': '"The equipment is not available. Current status: {equipment.Status}."',
    '"L\'équipement spécifié n\'existe pas."': '"The specified equipment does not exist."',
    '"Réservation annulée."': '"Reservation cancelled."',
    '"Réservation créée."': '"Reservation created."',
    '"Réservation mise à jour."': '"Reservation updated."',
    '"Réservation non trouvée."': '"Reservation not found."',
    '"Une erreur est survenue : {ex.Message}"': '"An error occurred: {ex.Message}"',
    '"Un utilisateur avec l\'email \'{email}\' existe déjà."': '"A user with the email \'{email}\' already exists."',
    '"Un utilisateur avec l\'email \'{user.Email}\' existe déjà."': '"A user with the email \'{user.Email}\' already exists."',
    '"Utilisateur créé avec succès."': '"User successfully created."',
    '"Utilisateur mis à jour."': '"User updated."',
    '"Utilisateur non trouvé."': '"User not found."',
    '"Utilisateur non trouvé ou échec de la suppression."': '"User not found or failed to delete."',
    '"Utilisateur supprimé."': '"User deleted."'
}

for root, _, files in os.walk(directory):
    for file in files:
        if file.endswith(".cs"):
            filepath = os.path.join(root, file)
            with open(filepath, 'r', encoding='utf-8') as f:
                content = f.read()
            
            changed = False
            for fr, en in replacements.items():
                if fr in content:
                    content = content.replace(fr, en)
                    changed = True
            
            # Auto-add inheritdoc to public methods in Service classes if missing
            if not file.startswith("I") and "Service : I" in content:
                # Add inheritdoc before 'public async Task'
                content = re.sub(r'(\s+)public async Task', r'\1/// <inheritdoc />\1public async Task', content)
                content = re.sub(r'(\s+)public Task', r'\1/// <inheritdoc />\1public Task', content)
                
                # Cleanup double inheritdoc
                content = content.replace("/// <inheritdoc />\n    /// <inheritdoc />", "/// <inheritdoc />")
                changed = True

            if changed:
                with open(filepath, 'w', encoding='utf-8') as f:
                    f.write(content)
                print(f"Updated {file}")
